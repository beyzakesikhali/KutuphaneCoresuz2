using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.Data
{
    public class Yazar
    {
        [Key]
        public int yazarID { get; set; }
        public string Isim { get; set; }
        public string Soyisim { get; set; }
        public string Yorum { get; set; }
  
    }
}
