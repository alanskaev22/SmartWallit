using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SmartWallit.Core.Interfaces;
using SmartWallit.Core.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SmartWallit.Infrastructure.Services
{
    public class EncryptionService : IEncryptionService
    {
        public HashSalt Encrypt(string value)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string encryptedValue = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: value,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));

            return new HashSalt { Hash = encryptedValue, Salt = salt };
        }

        public bool Decrypt(string enteredValue, byte[] salt, string hash)
        {
            string encryptedValue = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredValue,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));

            return encryptedValue.Equals(hash);
        }
    }
}
