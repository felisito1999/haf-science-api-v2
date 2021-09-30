using haf_science_api.Interfaces;
using haf_science_api.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace haf_science_api.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordOptions _passwordOptions;
        public PasswordService(IOptions<PasswordOptions> options)
        {
            _passwordOptions = options.Value;
        }
        public string ConvertSaltToString(byte[] salt)
        {
            return Convert.ToBase64String(salt);
        }
        public byte[] ConvertStringSaltToByteArray(string saltString)
        {
            return Convert.FromBase64String(saltString);
        }
        public byte[] GetSalt()
        {
            byte[] salt = new byte[128 / 8];

            using(var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            return salt;
        }

        public string HashPassword(string password, byte[] salt)
        {
            var value = _passwordOptions.Iterations;
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: _passwordOptions.Iterations,
                numBytesRequested: 256 / 8));

            return hashedPassword;
        }

        public string CreateDefaultUserPassword(string FirstNameLetters, string LastNameLetters, DateTime Birthdate)
        {
            return FirstNameLetters.Substring(0, 2).ToUpper() + Birthdate.Month.ToString()
                + LastNameLetters.Substring(0, 2).ToLower() + Birthdate.Year.ToString();
        }
    }
}
