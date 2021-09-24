using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Models
{
    public class TokenResponse
    {
        public string Status { get; set; }
        public string Token { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
