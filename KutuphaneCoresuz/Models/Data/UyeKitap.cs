using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.Data
{
    public class UyeKitap
    {
        [Key]
        public int ID { get; set; }
        public int aktiflik { get; set; }
        public int UyeID { get; set; }
        public int KitapID { get; set; }
        [ForeignKey("UyeID")]
        public virtual Uye Uye { get; set; }
        [ForeignKey("KitapID")]
        public virtual Kitap Kitap { get; set; }

        
      
    }
}