using KutuphaneCoresuz.Models.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.Data
{
    public class Kitap
    {
        [Key]

        public int ID { get; set; }
        [Display(Name = "Kitabın Adı:")]
        public string Isim { get; set; }
        [Display(Name = "Yayıncı:")]
        public string Yayinci { get; set; }
        [Display(Name = "Açıklama:")]
        public string Aciklama { get; set; }
        public ICollection<UyelerinKitaplari> UyeKitapFK { get; set; }
        public ICollection<YazarlarinKitaplari> YazarKitapFK { get; set; }



    }

}
