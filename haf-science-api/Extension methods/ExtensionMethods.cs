using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Extension_methods
{
    public static class ExtensionMethods
    {
        public static T? ToNullable<T>(this string str)
            where T : struct 
        {
            if(string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            return (T)Convert.ChangeType(str, typeof(T)); ;
        }
    }
}
