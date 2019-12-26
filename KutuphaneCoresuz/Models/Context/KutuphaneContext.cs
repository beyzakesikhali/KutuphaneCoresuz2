//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Data.Entity;
using KutuphaneCoresuz.Models.TableModels;
//using DbContext = System.Data.Entity.DbContext;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using KutuphaneCoresuz.Models.Data;

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
        public DbSet<YazarlarinKitaplari> YazarlarinKitaplariDb { get; set; }
        public DbSet<UyelerinKitaplari> UyelerinKitaplariDb { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<KutuphaneContext>(new DropCreateDatabaseAlways<KutuphaneContext>());
            modelBuilder.Entity<Kitap>().ToTable("Kitap");
            modelBuilder.Entity<Yazar>().ToTable("Yazar");

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