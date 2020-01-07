//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Data.Entity;
using KutuphaneCoresuz.Models.Data;
//using DbContext = System.Data.Entity.DbContext;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace KutuphaneCoresuz.Models.Context
{
    public class KutuphaneContext : DbContext
    {
        //public KutuphaneContext context = new KutuphaneContext();

        public KutuphaneContext() : base("KutuphaneSelammEntities")
        {
            //Database.SetInitializer<KutuphaneContext>(new KutuphaneVTInitializer());
            System.Data.Entity.Database.SetInitializer<KutuphaneContext>(new MigrateDatabaseToLatestVersion<KutuphaneContext, FepazoConfiguration>());
        }
       
        public DbSet<Uye> Uyeler { get; set; }
        public DbSet<Yazar> Yazarlar { get; set; }
        public DbSet<Kitap> Kitaplar { get; set; }
        public DbSet<UyeKitap> UyeKitap { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Yazar>().HasMany(yk => yk.YazarID).WithMany(yk=>yk.KitapID).Map(
            //    m =>
            //    {
            //        m.ToTable(YazarlarinKitaplari);
            //        m.MapLeftKey("YazarID");
            //        m.MapRightKey("KitapID");
            //    }
            //    );
            //modelBuilder.Entity<UyeKitap>().HasKey(uk => new { uk.KitapID, uk.UyeID });
            //modelBuilder




        }


    }

    public sealed class FepazoConfiguration : DbMigrationsConfiguration<KutuphaneContext>
    {
        public FepazoConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }

}