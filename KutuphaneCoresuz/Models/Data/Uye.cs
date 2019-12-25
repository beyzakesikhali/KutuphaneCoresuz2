using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.TableModels
{
    public class Uye
    {
        [Key]
        public int uyeID { get; set; }
        public string isim { get; set; }
        public string kAdi { get; set; }
        public string soyisim { get; set; }

        public string uye_sifre { get; set; }
        public string uye_email { get; set; }
        public string aciklama { get; set; }
      

    }
    public class UyelerLogin
    {
       
        public static bool KullaniciAdKontrol(string kullaniciAd)

        { 
            KutuphaneContext dbKontrol = new KutuphaneContext();
            return dbKontrol.Uyeler.Where(u => u.kAdi == kullaniciAd).Count() > 0 ? true : false;

        }
        public static List<Uye> UyelerInit()
        {
            return new List<Uye>
                {
                new Uye { uyeID = 1, kAdi = "beyza", uye_sifre = "pas1" } };
        }
    }

}
