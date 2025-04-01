using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VehicleRentingApi.Entities;

namespace VehicleRentingApi.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<RentalCar> RentalCars { get; set; }

    public virtual DbSet<RentalCarBooking> RentalCarBookings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MSI;Database=VehicleRenting;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8D96302F2");

            entity.Property(e => e.CustomerCity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RentalCar>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("PK__RentalCa__68A0342E245C90FD");

            entity.Property(e => e.Brand)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CarImage)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Model)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RegNo)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RentalCarBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__RentalCa__73951AED52597386");

            entity.Property(e => e.BookingDate).HasColumnType("datetime");
            entity.Property(e => e.BookingUid)
                .HasMaxLength(50)
                .IsUnicode(false);

            //entity.HasOne(d => d.Car).WithMany(p => p.RentalCarBookings)
            //    .HasForeignKey(d => d.CarId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__RentalCar__CarId__4222D4EF");

            //entity.HasOne(d => d.Cust).WithMany(p => p.RentalCarBookings)
            //    .HasForeignKey(d => d.CustId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__RentalCar__CustI__412EB0B6");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__Users__A25C5AA638E9855F");

            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
