using FluentValidation.Results;
using haf_science_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface IPasswordService
    {
        Task<string> HashPassword(string password, byte[] salt);
        Task<byte[]> GetSalt();
        Task<string> ConvertSaltToString(byte[] salt);
        Task<byte[]> ConvertStringSaltToByteArray(string saltString);
        Task<string> CreateDefaultUserPassword(string FirstNameLetters, string LastNameLetters, DateTime Birthdate);
        Task<ValidationResult> ValidatePassword(ChangePasswordModel password);
    }
}
