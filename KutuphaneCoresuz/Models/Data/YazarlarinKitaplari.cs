using KutuphaneCoresuz.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.Data
{
    public class YazarKitap
    {
        [Key]
        public int YazarKitapID { get; set; }

        public int KitapID { get; set; }
        public int YazarID { get; set; }

        [ForeignKey("YazarID")]
        public  Yazar Yazar { get; set; }
        [ForeignKey("KitapID")]
        public  Kitap Kitap { get; set; }

    }
}