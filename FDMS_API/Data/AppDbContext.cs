using FDMS_API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FDMS_API.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }
        public DbSet<User_Group> User_Groups { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Type_GroupPermission> Type_GroupPermissions { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<Confirmation> Confirmations { get; set; }
        public DbSet<DocumentPermission> DocumentPermissions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }
    }
}
