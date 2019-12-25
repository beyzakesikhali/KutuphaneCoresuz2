using KutuphaneCoresuz.Models.Context;
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
        [Display (Name ="Kitabın Adı:")]
        public string k_isim { get; set; }
        [Display(Name = "Yazarının Adı:")]
        public string yazar_adi { get; set; }
        public int uye_id { get; set; }
        [Display(Name = "Yayıncı:")]
        public int yayinci { get; set; }
        [Display(Name = "Açıklama:")]
        public string k_aciklama { get; set; }
        
        public bool KitapAdKontrol(string kullaniciAd)
        {
            KutuphaneContext dbKontrol = new KutuphaneContext();
            return dbKontrol.Uyeler.Where(u => u.kAdi == kullaniciAd).Count() > 0 ? true : false;
        }
    }

}
