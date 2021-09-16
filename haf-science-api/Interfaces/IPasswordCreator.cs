using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface IPasswordCreator
    {
        string CreateDefaultUserPassword(string FirstNameLetters, string LastNameLetters,
            DateTime Birthdate);
    }
}
