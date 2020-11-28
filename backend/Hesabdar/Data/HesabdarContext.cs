using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        public DbSet<Activity> Activity { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<Dealer> Dealer { get; set; }
        public DbSet<Deal> Deal { get; set; }
        public DbSet<DealItem> DealItem { get; set; }
        public DbSet<Payment> Payment { get; set; }


    }
}
