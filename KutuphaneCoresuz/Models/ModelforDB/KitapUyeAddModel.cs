using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.ModelforDB
{
    public class KitapUyeAddModel
    {
        public int Id { get; set; }

        public string KitapAdi { get; set; }

        public string UyeAdi { get; set; }

        public string UyeSoyadi { get; set; }
    }
}