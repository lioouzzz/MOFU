using Microsoft.EntityFrameworkCore;
using MOFU.Models;

namespace MOFU.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
    }
}
