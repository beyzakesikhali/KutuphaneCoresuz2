using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.TableModels
{
    public class Kitap
    {
        [Key]
        public int kitapID { get; set; }
        public string k_isim { get; set; }
        public string yazar_adi { get; set; }
        public int uye_id { get; set; }
        public int yazar_id { get; set; }
        public int yayinci { get; set; }
        public string k_aciklama { get; set; }
        public virtual ICollection<Yazar> Yazarlar { get; set; }
        public virtual ICollection<Uye> Kitaplar { get; set; }

    }
}