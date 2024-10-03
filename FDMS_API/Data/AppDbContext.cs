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
            // Foreign Key for User
            modelBuilder.Entity<User>(options =>
            {
                options.HasMany(u => u.Flights).WithOne(f => f.User).HasForeignKey(f => f.UserID).OnDelete(DeleteBehavior.NoAction);
                options.HasMany(u => u.Documents).WithOne(d => d.User).HasForeignKey(d => d.UserID).OnDelete(DeleteBehavior.NoAction);
                options.HasMany(u => u.DocumentTypes).WithOne(dt => dt.User).HasForeignKey(dt => dt.UserID).OnDelete(DeleteBehavior.NoAction);
                options.HasMany(u => u.Confirmations).WithOne(c => c.User).HasForeignKey(c => c.UserID).OnDelete(DeleteBehavior.NoAction);
                options.HasMany(u => u.SystemSettings).WithOne(s => s.User).HasForeignKey(s => s.UserID).OnDelete(DeleteBehavior.NoAction);
                options.HasMany(u => u.User_Groups).WithOne(ug => ug.User).HasForeignKey(ug => ug.UserID).OnDelete(DeleteBehavior.NoAction);
                options.HasMany(u => u.GroupPermission).WithOne(gp => gp.User).HasForeignKey(gp => gp.UserID).OnDelete(DeleteBehavior.NoAction);
            });
            // Foreign Key for Flight
            modelBuilder.Entity<Flight>(options =>
            {
                options.HasMany(f => f.Documents).WithOne(d=>d.Flight).HasForeignKey(d => d.FlightID).OnDelete(DeleteBehavior.NoAction);
                options.HasMany(f => f.Confirmations).WithOne(c => c.Flight).HasForeignKey(c => c.FlightID).OnDelete(DeleteBehavior.NoAction);
            });
            // Foreign Key for Document
            modelBuilder.Entity<Document>()
                .HasMany(d => d.DocumentPermissions).WithOne(dp => dp.Document).HasForeignKey(dp => dp.DocumentID).OnDelete(DeleteBehavior.NoAction);
            // Foreign Key for DocumentType
            modelBuilder.Entity<DocumentType>(options =>
            {
                options.HasMany(dt => dt.Documents).WithOne(d=>d.DocumentType).HasForeignKey(d => d.TypeID).OnDelete(DeleteBehavior.NoAction);
                options.HasMany(dt => dt.Type_GroupPermissions).WithOne(tgp=>tgp.DocumentType).HasForeignKey(tgp=>tgp.TypeID).OnDelete(DeleteBehavior.NoAction);
            });
            // Foreign Key for GroupPermission
            modelBuilder.Entity<GroupPermission>(options =>
            {
                options.HasMany(gp => gp.Type_GroupPermissions).WithOne(tgp=>tgp.GroupPermission).HasForeignKey(tgp=>tgp.GroupID).OnDelete(DeleteBehavior.NoAction);
                options.HasMany(dt => dt.User_Groups).WithOne(ug=>ug.GroupPermission).HasForeignKey(ug => ug.GroupID).OnDelete(DeleteBehavior.NoAction);
                options.HasMany(dt => dt.DocumentPermissions).WithOne(dp=>dp.GroupPermission).HasForeignKey(dp=>dp.GroupID).OnDelete(DeleteBehavior.NoAction);
            });

            // Primary Key for Type_GroupPermission, User_Group, DocumentPermission, Confirmation

            modelBuilder.Entity<Type_GroupPermission>().HasKey(tgp=>new {tgp.TypeID, tgp.GroupID});

            modelBuilder.Entity<User_Group>().HasKey(ug => new { ug.UserID, ug.GroupID });

            modelBuilder.Entity<DocumentPermission>().HasKey(dp => new { dp.DocumentID, dp.GroupID });

            modelBuilder.Entity<Confirmation>().HasKey(c => new { c.UserID, c.FlightID });


            // Dữ liệu mặc định khi setup CSDL

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    Name= "Admin default",
                    Email="Admin@gmail.com",
                    Phone="0898827656",
                    PasswordHash="1234567890",
                    IsTerminated=false,
                    Role = "Admin"
                }
                );
        }
    }
}
