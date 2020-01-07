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
    public class Yazar
    {
        [Key]
        public int ID { get; set; }
        public string Isim { get; set; }
        public string Soyisim { get; set; }
        public string Yorum { get; set; }
       
        public ICollection<Kitap> Kitaplar { get; set; }
       

    }
}
