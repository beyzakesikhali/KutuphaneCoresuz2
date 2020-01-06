using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.Data
{
    public class UyelerinKitaplari
    {
        [Key]
        public int UyeKitapID { get; set; }
        public int UyeID { get; set; }
        public int KitapID { get; set; }
       // [ForeignKey("UyeID")]
        public  Uye Uyeler { get; set; }
        //[ForeignKey("KitapID")]
        public  Kitap Kitaplar { get; set; }

    }
}