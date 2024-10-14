using FDMS_API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FDMS_API.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Models.Type> Types { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<DocumentPermission> DocumentPermissions { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Foreign Key for User
            modelBuilder.Entity<User>(options =>
            {
                options.HasMany(u => u.Flights).WithOne(f => f.User).HasForeignKey(f => f.UserID);
                options.HasMany(u => u.Documents).WithOne(d => d.User).HasForeignKey(d => d.UserID);
                options.HasMany(u => u.Types).WithOne(dt => dt.User).HasForeignKey(dt => dt.UserID);
                options.HasMany(u => u.Reports).WithOne(c => c.User).HasForeignKey(c => c.UserID);
                options.HasMany(u => u.GroupUsers).WithOne(ug => ug.User).HasForeignKey(ug => ug.UserID);
                options.HasMany(u => u.Groups).WithOne(gp => gp.User).HasForeignKey(gp => gp.UserID);
                options.HasMany(u => u.UserTokens).WithOne(t => t.User).HasForeignKey(gp => gp.UserID);
            });
            // Foreign Key for Flight
            modelBuilder.Entity<Flight>(options =>
            {
                options.HasMany(f => f.Documents).WithOne(d=>d.Flight).HasForeignKey(d => d.FlightID);
                options.HasMany(f => f.Reports).WithOne(c => c.Flight).HasForeignKey(c => c.FlightID);
            });
            // Foreign Key for Document
            modelBuilder.Entity<Document>()
                .HasMany(d => d.DocumentPermissions).WithOne(dp => dp.Document).HasForeignKey(dp => dp.DocumentID);
            // Foreign Key for DocumentType
            modelBuilder.Entity<Models.Type>(options =>
            {
                options.HasMany(dt => dt.Documents).WithOne(d=> d.Type).HasForeignKey(d => d.TypeID);
                options.HasMany(dt => dt.Permissions).WithOne(tgp=> tgp.Type).HasForeignKey(tgp=> tgp.TypeID);
            });
            // Foreign Key for Group
            modelBuilder.Entity<Group>(options =>
            {
                options.HasMany(gp => gp.Permissions).WithOne(tgp=>tgp.Group).HasForeignKey(tgp=>tgp.GroupID);
                options.HasMany(dt => dt.GroupUsers).WithOne(ug=>ug.Group).HasForeignKey(ug => ug.GroupID);
                options.HasMany(dt => dt.DocumentPermissions).WithOne(dp=>dp.Group).HasForeignKey(dp=>dp.GroupID);
            });

            // Primary Key for Type_GroupPermission, User_Group, DocumentPermission, Confirmation

            modelBuilder.Entity<Permission>().HasKey(tgp=>new {tgp.TypeID, tgp.GroupID});

            modelBuilder.Entity<GroupUser>().HasKey(ug => new { ug.UserID, ug.GroupID });

            modelBuilder.Entity<DocumentPermission>().HasKey(dp => new { dp.DocumentID, dp.GroupID });

            modelBuilder.Entity<Report>().HasKey(c => new { c.UserID, c.FlightID });


            // Chỉ định hành vi khi xóa 1 bảng ghi

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var foreignKeys = entityType.GetForeignKeys();
                foreach (var foreignKey in foreignKeys)
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
                }
            }

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
