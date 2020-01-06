using KutuphaneCoresuz.Models.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.Data
{
    public class Kitap
    {
        [Key]
        public int KitapID { get; set; }
        [Display(Name = "Kitabın Adı:")]
        public int UyeID { get; set; }
        public string Isim { get; set; }
        [Display(Name = "Yayıncı:")]
        public string Yayinci { get; set; }
        [Display(Name = "Açıklama:")]
        public string Aciklama { get; set; }


        ICollection<YazarKitap> YazarlarinKitaplari { get; set; }

        [ForeignKey("YazarID")]
        public virtual Yazar Yazar { get; set; }
        

        [ForeignKey("UyeID")]
        public virtual Uye Uye { get; set; }
        //public virtual ICollection<Uye> Uyeler { get; set; }
        //public virtual ICollection<Yazar> Yazarlar { get; set; }

        //FluentApi HasMany metod






    }

}
