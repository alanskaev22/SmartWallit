using SmartWallit.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWallit.Core.Interfaces
{
    public interface IEncryptionService
    {
        HashSalt Encrypt(string value);
        bool Decrypt(string unencryptedValue, byte[] salt, string hash);
    }
}
