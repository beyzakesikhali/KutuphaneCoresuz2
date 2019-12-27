using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.ModelforDB
{
    public class KitapYazarAddModel
    {

        public int Id { get; set; }

        public string KitapAdi { get; set; }
        
        public string YazarAdi { get; set; }

        public string YazarSoyadi { get; set; }
        public string yayinci { get; set; }

        public string Aciklama { get; set; }

    }
}