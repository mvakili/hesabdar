using Microsoft.EntityFrameworkCore;

namespace api.DataContext
{
    public class HesabdarContext : DbContext
    {
        public HesabdarContext(DbContextOptions<HesabdarContext> options) : base(options)
        {
        }

        public HesabdarContext() : base()
        {
        }
    }
}