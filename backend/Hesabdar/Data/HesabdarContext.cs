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

        }
        public DbSet<Hesabdar.Models.Activity> Activity { get; set; }
        public DbSet<Hesabdar.Models.Material> Material { get; set; }
        public DbSet<Hesabdar.Models.Dealer> Dealer { get; set; }
        public DbSet<Hesabdar.Models.Deal> Deal { get; set; }
        public DbSet<Hesabdar.Models.DealItem> DealItem { get; set; }



    }
}
