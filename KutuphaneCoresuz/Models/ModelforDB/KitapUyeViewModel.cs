using KutuphaneCoresuz.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KutuphaneCoresuz.Models.ModelforDB
{
    public class KitapUyeViewModel
    {
        public int id { get; set; }

        public List<Kitap> Kitaps  { get; set; }
    }
}