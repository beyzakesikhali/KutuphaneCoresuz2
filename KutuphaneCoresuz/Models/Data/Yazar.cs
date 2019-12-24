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
        public int kitap_id { get; set; }
        public virtual ICollection<Kitap> Kitaplar { get; set; }
    }
}