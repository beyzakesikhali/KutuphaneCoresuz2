using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace KutuphaneCoresuz.Helper
{
    public class SifreKontrol
    {
        public int SifreKontrolEt(string gelenSifre, string DbSifre)
        {

            int kontrol = 0;

            if (gelenSifre.Length > 0 && DbSifre.Length>0)
            {
                bool EsitMi = Crypto.VerifyHashedPassword(DbSifre, gelenSifre);
                if(EsitMi==true)
                {
                    kontrol = 1;//sifre dogru
                    return kontrol;

                }
                kontrol = 2;
                return kontrol;
              
            }
            kontrol = 3;
            return kontrol; //sifre yok
           
        }
      
    }
}