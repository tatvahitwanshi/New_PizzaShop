using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models;

public partial class PizzaShopContext : DbContext
{
    public PizzaShopContext()
    {
    }

    public PizzaShopContext(DbContextOptions<PizzaShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=Pizza_shop;Username=postgres;     password=Tatva@123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Cityid).HasName("city_pkey");

            entity.ToTable("city");

            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.Cityname)
                .HasMaxLength(50)
                .HasColumnName("cityname");
            entity.Property(e => e.Stateid).HasColumnName("stateid");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.Stateid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("city_stateid_fkey");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Countryid).HasName("country_pkey");

            entity.ToTable("country");

            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.Countryname)
                .HasMaxLength(50)
                .HasColumnName("countryname");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.EditDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("edit_date");
            entity.Property(e => e.EditedBy)
                .HasMaxLength(50)
                .HasColumnName("edited_by");
            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Stateid).HasName("state_pkey");

            entity.ToTable("state");

            entity.Property(e => e.Stateid).HasColumnName("stateid");
            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.Statename)
                .HasMaxLength(50)
                .HasColumnName("statename");

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.Countryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("state_countryid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.EditDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("edit_date");
            entity.Property(e => e.EditedBy)
                .HasMaxLength(50)
                .HasColumnName("edited_by");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(255)
                .HasColumnName("firstname");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .HasColumnName("lastname");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .HasColumnName("phone");
            entity.Property(e => e.Profilepic)
                .HasMaxLength(255)
                .HasColumnName("profilepic");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Stateid).HasColumnName("stateid");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .HasColumnName("username");
            entity.Property(e => e.Zipcode).HasColumnName("zipcode");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_roleid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
