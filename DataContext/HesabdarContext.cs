using api.Models.Data.Account;
using Microsoft.EntityFrameworkCore;

namespace api.DataContext
{
    public class HesabdarContext : DbContext
    {

        public DbSet<User> Users {get; set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Hesabdar;Integrated Security=True");
        }

        public HesabdarContext() : base()
        {
        }
    }
}