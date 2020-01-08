using KutuphaneCoresuz.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.ModelforDB
{
    public class KitapUyeViewModel
    {
        public int id { get; set; }
        public string UyeIsim { get; set; } 
        public string KullaniciAdi { get; set; }
            
        public string UyeSoyisim { get; set; }
        public string UyeSifre { get; set; }
        public string UyeEmail { get; set; }
        public string Aciklama { get; set; }
        public string KitapAdi { get; set; }
        public string yayinci { get; set; }
        public string YazarAdi { get; set; }
        public string YazarSoyadi { get; set; }
        public string YazarYorum { get; set; }
        
    }
}