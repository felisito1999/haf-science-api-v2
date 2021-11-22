using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class UserHash
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Hash { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public DateTime FechaCreacion { get; set; }

        public virtual Usuario User { get; set; }
    }
}
