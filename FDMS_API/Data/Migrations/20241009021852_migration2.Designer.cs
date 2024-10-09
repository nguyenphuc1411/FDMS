﻿// <auto-generated />
using System;
using FDMS_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FDMS_API.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241009021852_migration2")]
    partial class migration2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FDMS_API.Data.Models.Confirmation", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("FlightID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Confirmed_At")
                        .HasColumnType("datetime2");

                    b.Property<string>("SignatureURL")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserID", "FlightID");

                    b.HasIndex("FlightID");

                    b.ToTable("Confirmations");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Document", b =>
                {
                    b.Property<int>("DocumentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentID"));

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("FlightID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TypeID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<decimal>("Version")
                        .HasColumnType("decimal(2,1)");

                    b.HasKey("DocumentID");

                    b.HasIndex("FlightID");

                    b.HasIndex("TypeID");

                    b.HasIndex("UserID");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.DocumentPermission", b =>
                {
                    b.Property<int>("DocumentID")
                        .HasColumnType("int");

                    b.Property<int>("GroupID")
                        .HasColumnType("int");

                    b.HasKey("DocumentID", "GroupID");

                    b.HasIndex("GroupID");

                    b.ToTable("DocumentPermissions");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Flight", b =>
                {
                    b.Property<int>("FlightID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FlightID"));

                    b.Property<string>("AircraftID")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FlightNo")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("POL")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.Property<string>("POU")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("FlightID");

                    b.HasIndex("UserID");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Group", b =>
                {
                    b.Property<int>("GroupID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupID"));

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("GroupID");

                    b.HasIndex("UserID");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.GroupUser", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("GroupID")
                        .HasColumnType("int");

                    b.HasKey("UserID", "GroupID");

                    b.HasIndex("GroupID");

                    b.ToTable("GroupUsers");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Permission", b =>
                {
                    b.Property<int>("TypeID")
                        .HasColumnType("int");

                    b.Property<int>("GroupID")
                        .HasColumnType("int");

                    b.Property<bool>("CanEdit")
                        .HasColumnType("bit");

                    b.Property<bool>("CanRead")
                        .HasColumnType("bit");

                    b.HasKey("TypeID", "GroupID");

                    b.HasIndex("GroupID");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.SystemSetting", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<bool>("IsCaptchaRequired")
                        .HasColumnType("bit");

                    b.Property<string>("LogoURL")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Theme")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated_At")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("SystemSettings");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Type", b =>
                {
                    b.Property<int>("TypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TypeID"));

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("TypeID");

                    b.HasIndex("UserID");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsTerminated")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("varchar(11)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("UserID");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserID = 1,
                            Email = "Admin@gmail.com",
                            IsTerminated = false,
                            Name = "Admin default",
                            PasswordHash = "1234567890",
                            Phone = "0898827656",
                            Role = "Admin"
                        });
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Confirmation", b =>
                {
                    b.HasOne("FDMS_API.Data.Models.Flight", "Flight")
                        .WithMany("Confirmations")
                        .HasForeignKey("FlightID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FDMS_API.Data.Models.User", "User")
                        .WithMany("Confirmations")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Flight");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Document", b =>
                {
                    b.HasOne("FDMS_API.Data.Models.Flight", "Flight")
                        .WithMany("Documents")
                        .HasForeignKey("FlightID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FDMS_API.Data.Models.Type", "Type")
                        .WithMany("Documents")
                        .HasForeignKey("TypeID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FDMS_API.Data.Models.User", "User")
                        .WithMany("Documents")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Flight");

                    b.Navigation("Type");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.DocumentPermission", b =>
                {
                    b.HasOne("FDMS_API.Data.Models.Document", "Document")
                        .WithMany("DocumentPermissions")
                        .HasForeignKey("DocumentID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FDMS_API.Data.Models.Group", "Group")
                        .WithMany("DocumentPermissions")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Document");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Flight", b =>
                {
                    b.HasOne("FDMS_API.Data.Models.User", "User")
                        .WithMany("Flights")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Group", b =>
                {
                    b.HasOne("FDMS_API.Data.Models.User", "User")
                        .WithMany("Groups")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.GroupUser", b =>
                {
                    b.HasOne("FDMS_API.Data.Models.Group", "Group")
                        .WithMany("GroupUsers")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FDMS_API.Data.Models.User", "User")
                        .WithMany("GroupUsers")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Permission", b =>
                {
                    b.HasOne("FDMS_API.Data.Models.Group", "Group")
                        .WithMany("Permissions")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FDMS_API.Data.Models.Type", "Type")
                        .WithMany("Permissions")
                        .HasForeignKey("TypeID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.SystemSetting", b =>
                {
                    b.HasOne("FDMS_API.Data.Models.User", "User")
                        .WithMany("SystemSettings")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Type", b =>
                {
                    b.HasOne("FDMS_API.Data.Models.User", "User")
                        .WithMany("Types")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Document", b =>
                {
                    b.Navigation("DocumentPermissions");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Flight", b =>
                {
                    b.Navigation("Confirmations");

                    b.Navigation("Documents");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Group", b =>
                {
                    b.Navigation("DocumentPermissions");

                    b.Navigation("GroupUsers");

                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.Type", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("FDMS_API.Data.Models.User", b =>
                {
                    b.Navigation("Confirmations");

                    b.Navigation("Documents");

                    b.Navigation("Flights");

                    b.Navigation("GroupUsers");

                    b.Navigation("Groups");

                    b.Navigation("SystemSettings");

                    b.Navigation("Types");
                });
#pragma warning restore 612, 618
        }
    }
}
