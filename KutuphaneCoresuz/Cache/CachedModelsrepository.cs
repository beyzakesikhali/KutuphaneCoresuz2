using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static KutuphaneCoresuz.Cache.Cache;

namespace KutuphaneCoresuz.Cache
{
    public class CachedModelsrepository
    {
        protected KutuphaneContext DataContext { get; private set; }
        public CacheProvider Cache { get; set; }
        public CachedModelsrepository() : this(new DataCacheProvider())
         {    
           
        }

        public CachedModelsrepository(CacheProvider cacheProvider)
        {
            this.DataContext = new KutuphaneContext(); //EntityFramework
            this.Cache = cacheProvider;
        }
        public List<string> kitap()
        {
            List<string> kitapData = Cache.Get("Aciklamalar") as List<string>;
            if (kitapData == null)
            {
                kitapData = DataContext.Kitaplar.Select(c =>c.Aciklama).ToList();
                if (kitapData.Any())
                {
                    Cache.Set("Currencies", kitapData, 30);
                }
            }
            return kitapData;
        }
    }
}