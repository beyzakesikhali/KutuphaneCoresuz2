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
        public string KullaniciAdi { get; set; }
        public string Soyisim { get; set; }

        public string Sifre { get; set; }
        public string Email { get; set; }
        public string Aciklama { get; set; }
      

    }
    public class UyelerLogin
    {
       
       
        public static List<Uye> UyelerInit()
        {
            return new List<Uye>
                {
                new Uye { uyeID = 1, KullaniciAdi = "beyza", Sifre = "pas1" } };
        }
    }

}
