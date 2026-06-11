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
        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectMember> ProjectMember { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Users)   //一個 ProjectMember 只屬於一個 User
                .WithMany(u => u.ProjectMember)  // 一個 User 可以有多個 ProjectMember
                .HasForeignKey(pm => pm.UserId);

            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Project)
                .WithMany(pj => pj.ProjectMember)
                .HasForeignKey(pm=>pm.ProjectId);
        }
    }
}
