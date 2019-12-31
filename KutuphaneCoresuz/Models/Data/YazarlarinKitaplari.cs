using KutuphaneCoresuz.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.Data
{
    public class YazarlarinKitaplari
    {
        [Key]
        public int YazarKitapID { get; set; }
        public int YazarID { get; set; }
        public int KitapID { get; set; }
        public Yazar Yazarlar { get; set; }
        public Kitap Kitaplar { get; set; }

    }
}