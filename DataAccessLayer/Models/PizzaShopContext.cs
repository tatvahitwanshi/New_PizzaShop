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

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemsUnit> ItemsUnits { get; set; }

    public virtual DbSet<KotTable> KotTables { get; set; }

    public virtual DbSet<MapItemsModifiersgroup> MapItemsModifiersgroups { get; set; }

    public virtual DbSet<MapModifiersgroupModifier> MapModifiersgroupModifiers { get; set; }

    public virtual DbSet<MergeTable> MergeTables { get; set; }

    public virtual DbSet<Modifier> Modifiers { get; set; }

    public virtual DbSet<Modifiersgroup> Modifiersgroups { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderapp> Orderapps { get; set; }

    public virtual DbSet<Orderstatus> Orderstatuses { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Permissionlist> Permissionlists { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<Taxis> Taxes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WaitingTable> WaitingTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=Pizza_shop;Username=postgres;     password=Tatva@123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("category_pkey");

            entity.ToTable("category");

            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Categorydescription)
                .HasMaxLength(100)
                .HasColumnName("categorydescription");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(20)
                .HasColumnName("categoryname");
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
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
        });

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

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.AmbienceReview).HasColumnName("ambience_review");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasColumnName("comment");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CustomerDate)
                .HasMaxLength(30)
                .HasColumnName("customer_date");
            entity.Property(e => e.Customeremail)
                .HasMaxLength(20)
                .HasColumnName("customeremail");
            entity.Property(e => e.Customername)
                .HasMaxLength(15)
                .HasColumnName("customername");
            entity.Property(e => e.EditDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("edit_date");
            entity.Property(e => e.EditedBy)
                .HasMaxLength(50)
                .HasColumnName("edited_by");
            entity.Property(e => e.FoodReview).HasColumnName("food_review");
            entity.Property(e => e.Iswaiting)
                .HasDefaultValueSql("false")
                .HasColumnName("iswaiting");
            entity.Property(e => e.NumberOfPerson).HasColumnName("number_of_person");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.ServiceReview).HasColumnName("service_review");
            entity.Property(e => e.Totalorder)
                .HasMaxLength(255)
                .HasColumnName("totalorder");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Invoiceid).HasName("invoice_pkey");

            entity.ToTable("invoice");

            entity.Property(e => e.Invoiceid).HasColumnName("invoiceid");
            entity.Property(e => e.Invoicenumber)
                .HasMaxLength(20)
                .HasColumnName("invoicenumber");
            entity.Property(e => e.Orderappid).HasColumnName("orderappid");

            entity.HasOne(d => d.Orderapp).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.Orderappid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invoice_orderappid_fkey");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Itemid).HasName("items_pkey");

            entity.ToTable("items");

            entity.HasIndex(e => e.Itemname, "itemname").IsUnique();

            entity.Property(e => e.Itemid).HasColumnName("itemid");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Defaulttax)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("defaulttax");
            entity.Property(e => e.EditDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("edit_date");
            entity.Property(e => e.EditedBy)
                .HasMaxLength(50)
                .HasColumnName("edited_by");
            entity.Property(e => e.Isavailable)
                .HasDefaultValueSql("true")
                .HasColumnName("isavailable");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Itemdescription)
                .HasMaxLength(100)
                .HasColumnName("itemdescription");
            entity.Property(e => e.Itemimage)
                .HasMaxLength(255)
                .HasColumnName("itemimage");
            entity.Property(e => e.Itemname)
                .HasMaxLength(100)
                .HasColumnName("itemname");
            entity.Property(e => e.Itemtype)
                .HasMaxLength(20)
                .HasColumnName("itemtype");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.Shortcode)
                .HasMaxLength(10)
                .HasColumnName("shortcode");
            entity.Property(e => e.Taxesid).HasColumnName("taxesid");
            entity.Property(e => e.Taxpercentage)
                .HasPrecision(5, 2)
                .HasColumnName("taxpercentage");
            entity.Property(e => e.Unitid).HasColumnName("unitid");

            entity.HasOne(d => d.Category).WithMany(p => p.Items)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("items_categoryid_fkey");

            entity.HasOne(d => d.Taxes).WithMany(p => p.Items)
                .HasForeignKey(d => d.Taxesid)
                .HasConstraintName("items_taxesid_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.Items)
                .HasForeignKey(d => d.Unitid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("items_unitid_fkey");
        });

        modelBuilder.Entity<ItemsUnit>(entity =>
        {
            entity.HasKey(e => e.Unitid).HasName("items_unit_pkey");

            entity.ToTable("items_unit");

            entity.Property(e => e.Unitid).HasColumnName("unitid");
            entity.Property(e => e.Unitname)
                .HasMaxLength(50)
                .HasColumnName("unitname");
        });

        modelBuilder.Entity<KotTable>(entity =>
        {
            entity.HasKey(e => e.Kotid).HasName("kot_table_pkey");

            entity.ToTable("kot_table");

            entity.Property(e => e.Kotid).HasColumnName("kotid");
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
            entity.Property(e => e.ItemStatus).HasColumnName("item_status");
            entity.Property(e => e.OrderStatus).HasColumnName("order_status");
            entity.Property(e => e.Orderappid).HasColumnName("orderappid");

            entity.HasOne(d => d.Orderapp).WithMany(p => p.KotTables)
                .HasForeignKey(d => d.Orderappid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kot_table_orderappid_fkey");
        });

        modelBuilder.Entity<MapItemsModifiersgroup>(entity =>
        {
            entity.HasKey(e => e.Mergrid).HasName("map_items_modifiersgroup_pkey");

            entity.ToTable("map_items_modifiersgroup");

            entity.Property(e => e.Mergrid).HasColumnName("mergrid");
            entity.Property(e => e.Itemid).HasColumnName("itemid");
            entity.Property(e => e.Modifiersgroupid).HasColumnName("modifiersgroupid");

            entity.HasOne(d => d.Item).WithMany(p => p.MapItemsModifiersgroups)
                .HasForeignKey(d => d.Itemid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("map_items_modifiersgroup_itemid_fkey");

            entity.HasOne(d => d.Modifiersgroup).WithMany(p => p.MapItemsModifiersgroups)
                .HasForeignKey(d => d.Modifiersgroupid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("map_items_modifiersgroup_modifiersgroupid_fkey");
        });

        modelBuilder.Entity<MapModifiersgroupModifier>(entity =>
        {
            entity.HasKey(e => e.Mergrid).HasName("map_modifiersgroup_modifiers_pkey");

            entity.ToTable("map_modifiersgroup_modifiers");

            entity.Property(e => e.Mergrid).HasColumnName("mergrid");
            entity.Property(e => e.Modifiersgroupid).HasColumnName("modifiersgroupid");
            entity.Property(e => e.Modifiersid).HasColumnName("modifiersid");

            entity.HasOne(d => d.Modifiersgroup).WithMany(p => p.MapModifiersgroupModifiers)
                .HasForeignKey(d => d.Modifiersgroupid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("map_modifiersgroup_modifiers_modifiersgroupid_fkey");

            entity.HasOne(d => d.Modifiers).WithMany(p => p.MapModifiersgroupModifiers)
                .HasForeignKey(d => d.Modifiersid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("map_modifiersgroup_modifiers_modifiersid_fkey");
        });

        modelBuilder.Entity<MergeTable>(entity =>
        {
            entity.HasKey(e => e.Mergrid).HasName("merge_table_pkey");

            entity.ToTable("merge_table");

            entity.Property(e => e.Mergrid).HasColumnName("mergrid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Tablesid).HasColumnName("tablesid");

            entity.HasOne(d => d.Customer).WithMany(p => p.MergeTables)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("merge_table_customerid_fkey");

            entity.HasOne(d => d.Tables).WithMany(p => p.MergeTables)
                .HasForeignKey(d => d.Tablesid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("merge_table_tablesid_fkey");
        });

        modelBuilder.Entity<Modifier>(entity =>
        {
            entity.HasKey(e => e.Modifiersid).HasName("modifiers_pkey");

            entity.ToTable("modifiers");

            entity.Property(e => e.Modifiersid).HasColumnName("modifiersid");
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
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Modifiersdescription)
                .HasMaxLength(100)
                .HasColumnName("modifiersdescription");
            entity.Property(e => e.Modifiersname)
                .HasMaxLength(50)
                .HasColumnName("modifiersname");
            entity.Property(e => e.Modifiersquantity).HasColumnName("modifiersquantity");
            entity.Property(e => e.Modifiersrate).HasColumnName("modifiersrate");
            entity.Property(e => e.Modifiersunit).HasColumnName("modifiersunit");

            entity.HasOne(d => d.ModifiersunitNavigation).WithMany(p => p.Modifiers)
                .HasForeignKey(d => d.Modifiersunit)
                .HasConstraintName("modifiers_modifiersunit_fkey");
        });

        modelBuilder.Entity<Modifiersgroup>(entity =>
        {
            entity.HasKey(e => e.Modifiersgroupid).HasName("modifiersgroup_pkey");

            entity.ToTable("modifiersgroup");

            entity.Property(e => e.Modifiersgroupid).HasColumnName("modifiersgroupid");
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
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Modifiersgroupdescription)
                .HasMaxLength(100)
                .HasColumnName("modifiersgroupdescription");
            entity.Property(e => e.Modifiersgroupname)
                .HasMaxLength(20)
                .HasColumnName("modifiersgroupname");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("Order_pkey");

            entity.ToTable("Order");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.EditDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("edit_date");
            entity.Property(e => e.EditedBy)
                .HasMaxLength(50)
                .HasColumnName("edited_by");
            entity.Property(e => e.Orderstatusid).HasColumnName("orderstatusid");
            entity.Property(e => e.Paymentid).HasColumnName("paymentid");
            entity.Property(e => e.Rating)
                .HasDefaultValueSql("false")
                .HasColumnName("rating");
            entity.Property(e => e.Tablesid).HasColumnName("tablesid");
            entity.Property(e => e.Totalamount)
                .HasDefaultValueSql("true")
                .HasColumnName("totalamount");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_customerid_fkey");

            entity.HasOne(d => d.Orderstatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderstatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_orderstatusid_fkey");

            entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Paymentid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_paymentid_fkey");

            entity.HasOne(d => d.Tables).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Tablesid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_tablesid_fkey");
        });

        modelBuilder.Entity<Orderapp>(entity =>
        {
            entity.HasKey(e => e.Orderappid).HasName("orderapp_pkey");

            entity.ToTable("orderapp");

            entity.Property(e => e.Orderappid).HasColumnName("orderappid");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasColumnName("comment");
            entity.Property(e => e.ItemComment)
                .HasMaxLength(255)
                .HasColumnName("item_comment");
            entity.Property(e => e.Itemid).HasColumnName("itemid");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Item).WithMany(p => p.Orderapps)
                .HasForeignKey(d => d.Itemid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderapp_itemid_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderapps)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderapp_orderid_fkey");
        });

        modelBuilder.Entity<Orderstatus>(entity =>
        {
            entity.HasKey(e => e.Orderstatusid).HasName("orderstatus_pkey");

            entity.ToTable("orderstatus");

            entity.Property(e => e.Orderstatusid).HasColumnName("orderstatusid");
            entity.Property(e => e.Completed)
                .HasDefaultValueSql("(0)::bit(1)")
                .HasColumnType("bit(1)")
                .HasColumnName("completed");
            entity.Property(e => e.Inprogess)
                .HasDefaultValueSql("(0)::bit(1)")
                .HasColumnType("bit(1)")
                .HasColumnName("inprogess");
            entity.Property(e => e.Pending)
                .HasDefaultValueSql("(0)::bit(1)")
                .HasColumnType("bit(1)")
                .HasColumnName("pending");
            entity.Property(e => e.Running)
                .HasDefaultValueSql("(0)::bit(1)")
                .HasColumnType("bit(1)")
                .HasColumnName("running");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("payment_pkey");

            entity.ToTable("payment");

            entity.Property(e => e.Paymentid).HasColumnName("paymentid");
            entity.Property(e => e.Paymentmode)
                .HasMaxLength(20)
                .HasColumnName("paymentmode");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permission_pkey");

            entity.ToTable("permission");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('permission_permissionid_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Canaddedit)
                .HasDefaultValueSql("true")
                .HasColumnName("canaddedit");
            entity.Property(e => e.Candelete)
                .HasDefaultValueSql("true")
                .HasColumnName("candelete");
            entity.Property(e => e.Canview)
                .HasDefaultValueSql("true")
                .HasColumnName("canview");
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
            entity.Property(e => e.IsEnable).HasDefaultValueSql("true");
            entity.Property(e => e.Permissionid).HasColumnName("permissionid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");

            entity.HasOne(d => d.PermissionNavigation).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.Permissionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("permissionlist_roleid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("permission_roleid_fkey");
        });

        modelBuilder.Entity<Permissionlist>(entity =>
        {
            entity.HasKey(e => e.Permissionid).HasName("permissionlist_pkey");

            entity.ToTable("permissionlist");

            entity.Property(e => e.Permissionid).HasColumnName("permissionid");
            entity.Property(e => e.Permissionname)
                .HasMaxLength(50)
                .HasColumnName("permissionname");
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

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Sectionid).HasName("section_pkey");

            entity.ToTable("section");

            entity.Property(e => e.Sectionid).HasColumnName("sectionid");
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
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Sectiondescription)
                .HasMaxLength(100)
                .HasColumnName("sectiondescription");
            entity.Property(e => e.Sectionname)
                .HasMaxLength(20)
                .HasColumnName("sectionname");
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

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Tablesid).HasName("tables_pkey");

            entity.ToTable("tables");

            entity.Property(e => e.Tablesid).HasColumnName("tablesid");
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
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Isoccupied)
                .HasDefaultValueSql("false")
                .HasColumnName("isoccupied");
            entity.Property(e => e.Sectionid).HasColumnName("sectionid");
            entity.Property(e => e.Tablecapacity)
                .HasMaxLength(100)
                .HasColumnName("tablecapacity");
            entity.Property(e => e.Tablename)
                .HasMaxLength(20)
                .HasColumnName("tablename");

            entity.HasOne(d => d.Section).WithMany(p => p.Tables)
                .HasForeignKey(d => d.Sectionid)
                .HasConstraintName("tables_sectionid_fkey");
        });

        modelBuilder.Entity<Taxis>(entity =>
        {
            entity.HasKey(e => e.Taxesid).HasName("taxes_pkey");

            entity.ToTable("taxes");

            entity.Property(e => e.Taxesid).HasColumnName("taxesid");
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
            entity.Property(e => e.Isdefault)
                .HasDefaultValueSql("true")
                .HasColumnName("isdefault");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Isenabled)
                .HasDefaultValueSql("false")
                .HasColumnName("isenabled");
            entity.Property(e => e.Taxname)
                .HasMaxLength(20)
                .HasColumnName("taxname");
            entity.Property(e => e.Taxtype)
                .HasMaxLength(20)
                .HasColumnName("taxtype");
            entity.Property(e => e.Taxvalue).HasColumnName("taxvalue");
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
                .HasMaxLength(500)
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

        modelBuilder.Entity<WaitingTable>(entity =>
        {
            entity.HasKey(e => e.Waitingid).HasName("waiting_table_pkey");

            entity.ToTable("waiting_table");

            entity.Property(e => e.Waitingid).HasColumnName("waitingid");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.EditDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("edit_date");
            entity.Property(e => e.EditedBy)
                .HasMaxLength(50)
                .HasColumnName("edited_by");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Tableid).HasColumnName("tableid");

            entity.HasOne(d => d.Customer).WithMany(p => p.WaitingTables)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("waiting_table_customerid_fkey");

            entity.HasOne(d => d.Table).WithMany(p => p.WaitingTables)
                .HasForeignKey(d => d.Tableid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("waiting_table_tableid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
