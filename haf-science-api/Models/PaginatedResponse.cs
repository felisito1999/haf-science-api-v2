using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Models
{
    public class PaginatedResponse<T>
        where T : class
    {
        public IEnumerable<T> Records { get; set; }
        public int RecordsTotal { get; set; }
    }
}
