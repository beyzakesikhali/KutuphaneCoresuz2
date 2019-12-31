using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            public virtual Uye Uyeler { get; set; }
            public virtual Kitap Kitaplar { get; set; }

        }
    }