using Microsoft.EntityFrameworkCore;
using Proje1.FormModel;
using Proje1.Models;
using static Proje1.Models.SalesProducts;

namespace Proje1.DBContext
{
    public class SalesDBContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public SalesDBContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("myconn"));
        }

        public DbSet<SalesProducts> SalesProducts { get; set; }
        public DbSet<Saler> Saler { get; set; }
        public DbSet<Customer> Customer { get; set; }
    }
}
