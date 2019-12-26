using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using KutuphaneCoresuz.Models.Context;

namespace KutuphaneCoresuz.Models
{


    public class ExistControl
    {
        
        public int FKyazarid { get; set; }
        public int FKuyeid { get; set; }
        public string yazarAdi { get; set; }
        public string kitapAdi { get; set; }
        private KutuphaneContext dbKontrol = new KutuphaneContext();
        public bool YazarAdKontrol(string yazarAdi)
        {
            return dbKontrol.Uyeler.Where(u => u.KullaniciAdi == yazarAdi).Count() > 0 ? true : false;

        }
        public bool KullaniciAdKontrol(string kullaniciAd)

        {
            return dbKontrol.Uyeler.Where(u => u.KullaniciAdi == kullaniciAd).Count() > 0 ? true : false;

        }
        public bool KitapAdKontrol(string kitapAdi)
        {
            KutuphaneContext dbKontrol = new KutuphaneContext();
            return dbKontrol.Uyeler.Where(u => u.KullaniciAdi == kitapAdi).Count() > 0 ? true : false;
        }

    }


}
