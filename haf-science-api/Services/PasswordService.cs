using haf_science_api.Interfaces;
using haf_science_api.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using FluentValidation;
using haf_science_api.Models;
using FluentValidation.Results;

namespace haf_science_api.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordOptions _passwordOptions;
        private readonly IValidator<ChangePasswordModel> _passwordValidator;
        public PasswordService(IOptions<PasswordOptions> options, 
            IValidator<ChangePasswordModel> passwordValidator)
        {
            _passwordOptions = options.Value;
            _passwordValidator = passwordValidator;

        }
        public async Task<string> ConvertSaltToString(byte[] salt)
        {
            return await Task.Run(() => Convert.ToBase64String(salt));
        }
        public async Task<byte[]> ConvertStringSaltToByteArray(string saltString)
        {
            return await Task.Run(() => Convert.FromBase64String(saltString));
        }
        public async Task<byte[]> GetSalt()
        {
            var salt = await Task.Run(() =>
            {
                byte[] salt = new byte[128 / 8];

                using (var rngCsp = new RNGCryptoServiceProvider())
                {
                    rngCsp.GetNonZeroBytes(salt);
                }
                return salt;
            });
            
            return salt;
        }
        public async Task<string> HashPassword(string password, byte[] salt)
        {
            var hashedPassword = await Task.Run(() => Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: _passwordOptions.Iterations,
                numBytesRequested: 256 / 8)));

            return hashedPassword;
        }
        public async Task<string> CreateDefaultUserPassword(string FirstNameLetters, string LastNameLetters, DateTime Birthdate)
        {
            return await Task.Run(() => FirstNameLetters.Substring(0, 2).ToUpper() + Birthdate.Month.ToString()
                + LastNameLetters.Substring(0, 2).ToLower() + Birthdate.Year.ToString());
        }

        public async Task<ValidationResult> ValidatePassword(ChangePasswordModel password)
        {
            return await Task.Run(() => _passwordValidator.Validate(password));
        }
    }
}
