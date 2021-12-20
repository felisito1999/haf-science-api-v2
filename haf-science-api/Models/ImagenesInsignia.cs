using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class ImagenesInsignia
    {
        public ImagenesInsignia()
        {
            Insignia = new HashSet<Insignia>();
        }

        public int Id { get; set; }
        public string ImgData { get; set; }
        public string MimeType { get; set; }
        public bool Eliminado { get; set; }

        public virtual ICollection<Insignia> Insignia { get; set; }
    }
}
