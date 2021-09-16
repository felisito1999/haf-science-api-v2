using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password, byte[] salt);
        byte[] GetSalta();
        string ConvertSaltToString(byte[] salt);
    }
}
