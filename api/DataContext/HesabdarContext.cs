using System.Linq;
using api.Models.Data.Account;
using api.Models.Data.Materials;
using api.Models.Data.Transaction;
using Microsoft.EntityFrameworkCore;

namespace api.DataContext
{
    public class HesabdarContext : DbContext
    {

        public DbSet<User> Users {get; set;}
        public DbSet<Shop> Shops {get; set;}
        public DbSet<Material> Materials {get; set;}
        public DbSet<Unit> Units {get; set;}
        public DbSet<FinancialInvoice> FinancialInvoices {get; set;}
        public DbSet<MaterialInvoice> MaterialInvoices {get; set;}
        public DbSet<MaterialInvoiceItem> MaterialInvoiceItems {get; set;}

        public IQueryable<User> ActiveUsers => this.Users.Where(u => u.Deleted == false);
        public IQueryable<Shop> ActiveShops => this.Shops.Where(u => u.Deleted == false);
        public IQueryable<Material> ActiveMaterials => this.Materials.Where(u => u.Deleted == false);
        public IQueryable<Unit> ActiveUnits => this.Units.Where(u => u.Deleted == false);
        public IQueryable<FinancialInvoice> ActiveFinancialInvoices => this.FinancialInvoices.Where(u => u.Deleted == false);
        public IQueryable<MaterialInvoice> ActiveMaterialInvoices => this.MaterialInvoices.Where(u => u.Deleted == false);
        public IQueryable<MaterialInvoiceItem> ActiveMaterialInvoiceItems => this.MaterialInvoiceItems.Where(u => u.Deleted == false);
        
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Hesabdar;Integrated Security=True");
        }

        public HesabdarContext() : base()
        {
        }
    }
}