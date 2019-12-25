using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.TableModels
{
    public class Yazar
    {
        [Key]
        public int yazarID { get; set; }
        public string yazar_ismi { get; set; }
        public string yazar_sifre { get; set; }
        public string yazar_email { get; set; }
        
        public bool YazarAdKontrol(string kullaniciAd)

        {

            KutuphaneContext dbKontrol = new KutuphaneContext();

            return dbKontrol.Uyeler.Where(u => u.kAdi == kullaniciAd).Count() > 0 ? true : false;

        }
    }
}
