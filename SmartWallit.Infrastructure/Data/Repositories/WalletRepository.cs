using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartWallit.Core.Entities;
using SmartWallit.Core.Entities.Identity;
using SmartWallit.Core.Exceptions;
using SmartWallit.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartWallit.Infrastructure.Data.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletContext _walletContext;
        private readonly UserManager<AppUser> _userManager;

        private Object ThreadLock = new Object();
        public WalletRepository(WalletContext walletContext, UserManager<AppUser> userManager)
        {
            _walletContext = walletContext;
            _userManager = userManager;
        }

        public async Task<WalletEntity> CreateWallet(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new CustomException(System.Net.HttpStatusCode.BadRequest, $"No user was found with id {userId}.");
            }

            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet != null)
            {
                throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Wallet already exists for this user.");
            }

            wallet = new WalletEntity { UserId = userId };

            await _walletContext.Wallets.AddAsync(wallet);
            await _walletContext.SaveChangesAsync();

            return wallet;
        }

        public async Task<WalletEntity> GetWallet(string userId)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            return wallet ?? throw new CustomException(System.Net.HttpStatusCode.NotFound, "No wallet was found for this user.");
        }

        public async Task<bool> DeleteWallet(string userId, CancellationToken cancellationToken)
        {
            var success = false;

            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            var cards = await _walletContext.Cards.Where(c => c.WalletId == wallet.Id).ToListAsync();

            var executionStrategy = _walletContext.Database.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(
                    async () =>
                    {

                        using (var transaction = await _walletContext.Database.BeginTransactionAsync(cancellationToken))
                        {
                            try
                            {
                                _walletContext.Cards.RemoveRange(cards);

                                _walletContext.Wallets.Remove(wallet);

                                await _walletContext.SaveChangesAsync();

                                // Wait on purpose to demonstrate cancellation works and allow user to change their mind;
                                await Task.Delay(3000);

                                await transaction.CommitAsync(cancellationToken);

                                success = true;
                            }
                            catch (TaskCanceledException ex)
                            {
                                throw new Exception("Operation was cancelled by user.", ex);
                            }
                            catch
                            {
                                throw;
                            }
                        }
                    });

            return success;
        }

        public async Task<WalletEntity> AddFunds(string userId, int cardId, decimal amount, string email, CancellationToken cancellationToken)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            var card = await _walletContext.Cards.FindAsync(cardId);

            if (wallet == null || card == null) throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Bad Request");

            if (wallet.Id != card.WalletId) throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Invalid Card.");

            lock (ThreadLock)
            {
                wallet.Balance += Math.Abs(amount);

                var transaction = new TransactionEntity
                {
                    Amount = Math.Abs(amount),
                    WalletId = wallet.Id,
                    CardId = card.Id,
                    Email = email,
                    CardNumber = card.CardNumber
                };
                var executionStrategy = _walletContext.Database.CreateExecutionStrategy();

                executionStrategy.Execute(
                    () =>
                    {
                        using var dbTrans = _walletContext.Database.BeginTransaction();
                        try
                        {
                            _walletContext.Wallets.Update(wallet);

                            _walletContext.Transactions.Add(transaction);

                            _walletContext.SaveChanges();

                        // Wait on purpose to demonstrate cancellation works and allow user to change their mind;
                        Thread.Sleep(3000);

                            cancellationToken.ThrowIfCancellationRequested();

                            dbTrans.Commit();
                        }
                        catch (OperationCanceledException ex)
                        {
                            throw new Exception("Operation was cancelled by user.", ex);
                        }
                        catch
                        {
                            throw new CustomException(System.Net.HttpStatusCode.InternalServerError, "Unable to add funds. No funds have been transferred from the card.");
                        }

                    }
                    );
            }

            return wallet;
        }

        public async Task<WalletEntity> WithdrawFunds(string userId, int cardId, decimal amount, string email, CancellationToken cancellationToken)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            var card = await _walletContext.Cards.FindAsync(cardId);

            if (wallet == null || card == null) throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Bad Request.");

            if (wallet.Id != card.WalletId) throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Invalid Card.");

            if (wallet.Balance < Math.Abs(amount)) throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Cannot withdaraw more than current wallet balance.");

            lock (ThreadLock)
            {
                wallet.Balance -= Math.Abs(amount);

                var transaction = new TransactionEntity
                {
                    Amount = -Math.Abs(amount),
                    WalletId = wallet.Id,
                    CardId = card.Id,
                    Email = email,
                    CardNumber = card.CardNumber
                };
                var executionStrategy = _walletContext.Database.CreateExecutionStrategy();

                executionStrategy.Execute(
                    () =>
                    {
                        using var dbTrans = _walletContext.Database.BeginTransaction();
                        try
                        {
                            _walletContext.Wallets.Update(wallet);

                            _walletContext.Transactions.Add(transaction);

                            _walletContext.SaveChanges();

                        // Wait on purpose to demonstrate cancellation works and allow user to change their mind;
                        Thread.Sleep(3000);

                            cancellationToken.ThrowIfCancellationRequested();

                            dbTrans.Commit();
                        }
                        catch (OperationCanceledException ex)
                        {
                            throw new Exception("Operation was cancelled by user.", ex);
                        }
                        catch
                        {
                            throw new CustomException(System.Net.HttpStatusCode.InternalServerError, "Unable to withdraw funds. No funds have been transferred from the wallet.");
                        }
                    });
            }

            return wallet;
        }

        public async Task<List<TransactionEntity>> GetTransactions(string userId)
        {
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null) throw new CustomException(System.Net.HttpStatusCode.NotFound, "No transactions found.");

            var transactions = await _walletContext.Transactions.Where(t => t.WalletId == wallet.Id).ToListAsync();

            return transactions;
        }
    }
}
