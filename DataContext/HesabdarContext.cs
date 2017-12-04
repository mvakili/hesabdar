using api.Models.Data.Account;
using api.Models.Data.Material;
using api.Models.Data.Store;
using Microsoft.EntityFrameworkCore;

namespace api.DataContext
{
    public class HesabdarContext : DbContext
    {

        public DbSet<User> Users {get; set;}
        public DbSet<Store> Stores {get; set;}
        public DbSet<Material> Materials {get; set;}
        public DbSet<Unit> Units {get; set;}
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Hesabdar;Integrated Security=True");
        }

        public HesabdarContext() : base()
        {
        }
    }
}