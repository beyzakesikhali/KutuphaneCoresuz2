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
        public int ID { get; set; }
        [Display(Name = "Kitabın Adı:")]
        public int YazarID { get; set; }
        public string Isim { get; set; }
        [Display(Name = "Yayıncı:")]
        public string Yayinci { get; set; }
        [Display(Name = "Açıklama:")]
        public string Aciklama { get; set; }
        public int KitapDurum { get; set; }
        ICollection<UyeKitap> UyeKitap { get; set; }
        [ForeignKey("YazarID")]
        public virtual Yazar Yazar { get; set; }
       
     

    }

}
