using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(string password, byte[] salt);
        byte[] GetSalt();
        string ConvertSaltToString(byte[] salt);
        byte[] ConvertStringSaltToByteArray(string saltString);
        string CreateDefaultUserPassword(string FirstNameLetters, string LastNameLetters, DateTime Birthdate);
    }
}
