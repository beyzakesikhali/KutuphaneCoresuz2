using KutuphaneCoresuz.Models.TableModels;
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
        public virtual ICollection<Yazar> Yazarlar { get; set; }
        public virtual ICollection<Kitap> Kitaplar { get; set; }

    }
}