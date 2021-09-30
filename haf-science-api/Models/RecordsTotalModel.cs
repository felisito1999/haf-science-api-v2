using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Models
{
    [Keyless]
    public class RecordsTotalModel
    {
        public int RecordsTotal { get; set; }
    }
}
