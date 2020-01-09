using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.Data
{
    public class Uye
    {
        [Key]
        public int ID { get; set; }
        public string isim { get; set; }
        public string KullaniciAdi { get; set; }
        public string Soyisim { get; set; }
        public string Sifre { get; set; }
        public string Email { get; set; }
        public string Aciklama { get; set; }
        public int RoleId { get; set; }
        public virtual ICollection<UyeKitap> UyeKitaplar { get; set; }
    }
    public class UyelerLogin
    {
        public static List<Uye> AdminInit()
        {
            return new List<Uye>
            {
                new Uye { ID = 1, KullaniciAdi = "beyza", Sifre = "Allah Bana Yeter" } };
            }
    }

}
