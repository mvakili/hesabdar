using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hesabdar.Models;

namespace Hesabdar.Models
{
    public class HesabdarContext : DbContext
    {
        public HesabdarContext(DbContextOptions<HesabdarContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Deal>()
                .Property(b => b.DealTime)
                .HasDefaultValueSql("getdate()");

            var cascadeFKs = builder.Model.GetEntityTypes().
                SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;


            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal)))
            {
                property.Relational().ColumnType = "decimal(15, 3)";
            }
            base.OnModelCreating(builder);


        }
        public DbSet<Hesabdar.Models.Activity> Activity { get; set; }
        public DbSet<Hesabdar.Models.Material> Material { get; set; }
        public DbSet<Hesabdar.Models.Dealer> Dealer { get; set; }
        public DbSet<Hesabdar.Models.Deal> Deal { get; set; }
        public DbSet<Hesabdar.Models.DealItem> DealItem { get; set; }
        public DbSet<Hesabdar.Models.Payment> Payment { get; set; }



    }
}
