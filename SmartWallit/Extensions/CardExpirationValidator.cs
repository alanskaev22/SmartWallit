using SmartWallit.Core.Exceptions;
using SmartWallit.Models;
using System;

namespace SmartWallit.Extensions
{
    public static class CardExpirationValidator
    {
        public static void ValidateCardExpiration<T>(this T cardRequest) where T : BaseCard
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            if (cardRequest.ExpirationYear < currentYear) throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Invalid Expiration Year.");

            if (cardRequest.ExpirationYear == currentYear && cardRequest.ExpirationMonth < currentMonth) throw new CustomException(System.Net.HttpStatusCode.BadRequest, "Invalid Expiration Month");

        }
    }
}
