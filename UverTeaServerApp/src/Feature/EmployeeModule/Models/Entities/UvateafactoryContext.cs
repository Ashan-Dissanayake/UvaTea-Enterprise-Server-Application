using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class UvateafactoryContext : DbContext
{
    public UvateafactoryContext(DbContextOptions<UvateafactoryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Areacategory> Areacategories { get; set; }

    public virtual DbSet<Areastatus> Areastatuses { get; set; }

    public virtual DbSet<Designation> Designations { get; set; }

    public virtual DbSet<Distributor> Distributors { get; set; }

    public virtual DbSet<Distributorstatus> Distributorstatuses { get; set; }

    public virtual DbSet<Distributortype> Distributortypes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Employeestatus> Employeestatuses { get; set; }

    public virtual DbSet<Ferdistributionstate> Ferdistributionstates { get; set; }

    public virtual DbSet<Fertilizer> Fertilizers { get; set; }

    public virtual DbSet<Fertilizerdistribution> Fertilizerdistributions { get; set; }

    public virtual DbSet<Fertilizerstatus> Fertilizerstatuses { get; set; }

    public virtual DbSet<Fertilizertype> Fertilizertypes { get; set; }

    public virtual DbSet<Fertilzerbrand> Fertilzerbrands { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Invoiceproduct> Invoiceproducts { get; set; }

    public virtual DbSet<Invoicestatus> Invoicestatuses { get; set; }

    public virtual DbSet<Leaftype> Leaftypes { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Operation> Operations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderrproduct> Orderrproducts { get; set; }

    public virtual DbSet<Orderstatus> Orderstatuses { get; set; }

    public virtual DbSet<Plucking> Pluckings { get; set; }

    public virtual DbSet<Pluckingseesion> Pluckingseesions { get; set; }

    public virtual DbSet<Privilage> Privilages { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Production> Productions { get; set; }

    public virtual DbSet<Productionorder> Productionorders { get; set; }

    public virtual DbSet<Productionorderstatus> Productionorderstatuses { get; set; }

    public virtual DbSet<Productionproduct> Productionproducts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userstatus> Userstatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__area__3213E83F09142B09");

            entity.ToTable("area", "uvateafactory");

            entity.HasIndex(e => e.AreacategoryId, "fk_area_areacategory1_idx");

            entity.HasIndex(e => e.AreastatusId, "fk_area_areastatus1_idx");

            entity.HasIndex(e => e.SupervisorId, "fk_area_employee1_idx");

            entity.HasIndex(e => e.UserId, "fk_area_user1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Acres)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("acres");
            entity.Property(e => e.AreacategoryId).HasColumnName("areacategory_id");
            entity.Property(e => e.AreastatusId).HasColumnName("areastatus_id");
            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code");
            entity.Property(e => e.Doattached).HasColumnName("doattached");
            entity.Property(e => e.Doproofing).HasColumnName("doproofing");
            entity.Property(e => e.Plantcount).HasColumnName("plantcount");
            entity.Property(e => e.SupervisorId).HasColumnName("supervisor_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Areacategory).WithMany(p => p.Areas)
                .HasForeignKey(d => d.AreacategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_area_areacategory1");

            entity.HasOne(d => d.Areastatus).WithMany(p => p.Areas)
                .HasForeignKey(d => d.AreastatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_area_areastatus1");

            entity.HasOne(d => d.Supervisor).WithMany(p => p.Areas)
                .HasForeignKey(d => d.SupervisorId)
                .HasConstraintName("fk_area_employee1");

            entity.HasOne(d => d.User).WithMany(p => p.Areas)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_area_user1");
        });

        modelBuilder.Entity<Areacategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__areacate__3213E83F38E8776A");

            entity.ToTable("areacategory", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Areastatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__areastat__3213E83F707B3463");

            entity.ToTable("areastatus", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Designation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__designat__3213E83FD9610CC4");

            entity.ToTable("designation", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Distributor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__distribu__3213E83F998096E8");

            entity.ToTable("distributor", "uvateafactory");

            entity.HasIndex(e => e.DistributorstatusId, "fk_distributor_distributorstatus1_idx");

            entity.HasIndex(e => e.DistributortypeId, "fk_distributor_distributortype1_idx");

            entity.HasIndex(e => e.UserId, "fk_distributor_user1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasColumnType("text")
                .HasColumnName("address");
            entity.Property(e => e.Contactperson)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("contactperson");
            entity.Property(e => e.Contactpersontp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("contactpersontp");
            entity.Property(e => e.Creditlimit)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("creditlimit");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.DistributorstatusId).HasColumnName("distributorstatus_id");
            entity.Property(e => e.DistributortypeId).HasColumnName("distributortype_id");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Telephone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("telephone");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Distributorstatus).WithMany(p => p.Distributors)
                .HasForeignKey(d => d.DistributorstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_distributor_distributorstatus1");

            entity.HasOne(d => d.Distributortype).WithMany(p => p.Distributors)
                .HasForeignKey(d => d.DistributortypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_distributor_distributortype1");

            entity.HasOne(d => d.User).WithMany(p => p.Distributors)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_distributor_user1");
        });

        modelBuilder.Entity<Distributorstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__distribu__3213E83F53DD3278");

            entity.ToTable("distributorstatus", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Distributortype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__distribu__3213E83FE1AC4810");

            entity.ToTable("distributortype", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83F329AEA98");

            entity.ToTable("employee", "uvateafactory");

            entity.HasIndex(e => e.DesignationId, "fk_employee_designation1_idx");

            entity.HasIndex(e => e.EmployeestatusId, "fk_employee_employeestatus1_idx");

            entity.HasIndex(e => e.GenderId, "fk_employee_gender_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasColumnType("text")
                .HasColumnName("address");
            entity.Property(e => e.Callingname)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("callingname");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.DesignationId).HasColumnName("designation_id");
            entity.Property(e => e.Doassignment).HasColumnName("doassignment");
            entity.Property(e => e.Dobirth).HasColumnName("dobirth");
            entity.Property(e => e.EmployeestatusId).HasColumnName("employeestatus_id");
            entity.Property(e => e.Fullname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("fullname");
            entity.Property(e => e.GenderId).HasColumnName("gender_id");
            entity.Property(e => e.Land)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("land");
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("mobile");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nic)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("nic");
            entity.Property(e => e.Number)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("number");

            entity.HasOne(d => d.Designation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DesignationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employee_designation1");

            entity.HasOne(d => d.Employeestatus).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeestatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employee_employeestatus1");

            entity.HasOne(d => d.Gender).WithMany(p => p.Employees)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employee_gender");
        });

        modelBuilder.Entity<Employeestatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83FE72E1F60");

            entity.ToTable("employeestatus", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Ferdistributionstate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ferdistr__3213E83F654102E4");

            entity.ToTable("ferdistributionstate", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Fertilizer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__fertiliz__3213E83F9ACC1DFA");

            entity.ToTable("fertilizer", "uvateafactory");

            entity.HasIndex(e => e.BrandId, "fk_fertilizer_brand1_idx");

            entity.HasIndex(e => e.FertilizerstatusId, "fk_fertilizer_fertilizerstatus1_idx");

            entity.HasIndex(e => e.FertilizertypeId, "fk_fertilizer_fertilizertype1_idx");

            entity.HasIndex(e => e.UserId, "fk_fertilizer_user1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.Dointroduced).HasColumnName("dointroduced");
            entity.Property(e => e.FertilizerstatusId).HasColumnName("fertilizerstatus_id");
            entity.Property(e => e.FertilizertypeId).HasColumnName("fertilizertype_id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Quantity)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("quantity");
            entity.Property(e => e.Rop)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("rop");
            entity.Property(e => e.Unitprice)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("unitprice");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Brand).WithMany(p => p.Fertilizers)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fertilizer_brand1");

            entity.HasOne(d => d.Fertilizerstatus).WithMany(p => p.Fertilizers)
                .HasForeignKey(d => d.FertilizerstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fertilizer_fertilizerstatus1");

            entity.HasOne(d => d.Fertilizertype).WithMany(p => p.Fertilizers)
                .HasForeignKey(d => d.FertilizertypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fertilizer_fertilizertype1");

            entity.HasOne(d => d.User).WithMany(p => p.Fertilizers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fertilizer_user1");
        });

        modelBuilder.Entity<Fertilizerdistribution>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__fertiliz__3213E83F5BD83F1C");

            entity.ToTable("fertilizerdistribution", "uvateafactory");

            entity.HasIndex(e => e.AreaId, "fk_fertilizerdistribution_area1_idx");

            entity.HasIndex(e => e.FerdistributionstateId, "fk_fertilizerdistribution_ferdistributionstate1_idx");

            entity.HasIndex(e => e.FertilizerId, "fk_fertilizerdistribution_fertilizer1_idx");

            entity.HasIndex(e => e.UserId, "fk_fertilizerdistribution_user1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.FerdistributionstateId).HasColumnName("ferdistributionstate_id");
            entity.Property(e => e.FertilizerId).HasColumnName("fertilizer_id");
            entity.Property(e => e.Quantity)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("quantity");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Area).WithMany(p => p.Fertilizerdistributions)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("fk_fertilizerdistribution_area1");

            entity.HasOne(d => d.Ferdistributionstate).WithMany(p => p.Fertilizerdistributions)
                .HasForeignKey(d => d.FerdistributionstateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fertilizerdistribution_ferdistributionstate1");

            entity.HasOne(d => d.Fertilizer).WithMany(p => p.Fertilizerdistributions)
                .HasForeignKey(d => d.FertilizerId)
                .HasConstraintName("fk_fertilizerdistribution_fertilizer1");

            entity.HasOne(d => d.User).WithMany(p => p.Fertilizerdistributions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fertilizerdistribution_user1");
        });

        modelBuilder.Entity<Fertilizerstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__fertiliz__3213E83FDFE47FA5");

            entity.ToTable("fertilizerstatus", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Fertilizertype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__fertiliz__3213E83FBC0C909D");

            entity.ToTable("fertilizertype", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Fertilzerbrand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__fertilze__3213E83FA9C45DD0");

            entity.ToTable("fertilzerbrand", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__gender__3213E83F0720E997");

            entity.ToTable("gender", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__invoice__3213E83FA5442392");

            entity.ToTable("invoice", "uvateafactory");

            entity.HasIndex(e => e.InvoicestatusId, "fk_invoice_invoicestatus1_idx");

            entity.HasIndex(e => e.OrderId, "fk_invoice_order1_idx");

            entity.HasIndex(e => e.UserId, "fk_invoice_user1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Grandtotal)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("grandtotal");
            entity.Property(e => e.InvoicestatusId).HasColumnName("invoicestatus_id");
            entity.Property(e => e.Number)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("number");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Invoicestatus).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.InvoicestatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoice_invoicestatus1");

            entity.HasOne(d => d.Order).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoice_order1");

            entity.HasOne(d => d.User).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoice_user1");
        });

        modelBuilder.Entity<Invoiceproduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__invoicep__3213E83F4A86FD57");

            entity.ToTable("invoiceproduct", "uvateafactory");

            entity.HasIndex(e => e.InvoiceId, "fk_invoice_has_product_invoice1_idx");

            entity.HasIndex(e => e.ProductId, "fk_invoice_has_product_product1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.Linetotal)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("linetotal");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Qty)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("qty");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Invoiceproducts)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoice_has_product_invoice1");

            entity.HasOne(d => d.Product).WithMany(p => p.Invoiceproducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoice_has_product_product1");
        });

        modelBuilder.Entity<Invoicestatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__invoices__3213E83F9F715F7E");

            entity.ToTable("invoicestatus", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Leaftype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__leaftype__3213E83F300FFD3E");

            entity.ToTable("leaftype", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__module__3213E83F30DB15DB");

            entity.ToTable("module", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Operation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__operatio__3213E83F49279246");

            entity.ToTable("operation", "uvateafactory");

            entity.HasIndex(e => e.ModuleId, "fk_operation_module1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ModuleId).HasColumnName("module_id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.Module).WithMany(p => p.Operations)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("fk_operation_module1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order__3213E83F511936F2");

            entity.ToTable("order", "uvateafactory");

            entity.HasIndex(e => e.DistributorId, "fk_order_distributor1_idx");

            entity.HasIndex(e => e.UserId, "fk_order_user1_idx");

            entity.HasIndex(e => e.OrderstatusId, "fk_orderr_orderstatus1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DistributorId).HasColumnName("distributor_id");
            entity.Property(e => e.Doexpected).HasColumnName("doexpected");
            entity.Property(e => e.Doorder).HasColumnName("doorder");
            entity.Property(e => e.Expectedgrandtotal)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("expectedgrandtotal");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.OrderstatusId).HasColumnName("orderstatus_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Distributor).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DistributorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_distributor1");

            entity.HasOne(d => d.Orderstatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderr_orderstatus1");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_user1");
        });

        modelBuilder.Entity<Orderrproduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__orderrpr__3213E83F56AFE11D");

            entity.ToTable("orderrproduct", "uvateafactory");

            entity.HasIndex(e => e.ProductId, "fk_orderr_has_product_product1_idx");

            entity.HasIndex(e => e.OrderId, "fk_orderrproduct_order1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Linetotal)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("linetotal");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Qty)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("qty");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderrproducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderrproduct_order1");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderrproducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderr_has_product_product1");
        });

        modelBuilder.Entity<Orderstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ordersta__3213E83F1FDB31A9");

            entity.ToTable("orderstatus", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Plucking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__plucking__3213E83FC8FE284D");

            entity.ToTable("plucking", "uvateafactory");

            entity.HasIndex(e => e.AreaId, "fk_plucking_area1_idx");

            entity.HasIndex(e => e.PluckerId, "fk_plucking_employee1_idx");

            entity.HasIndex(e => e.LeaftypeId, "fk_plucking_leaftype1_idx");

            entity.HasIndex(e => e.PluckingseesionId, "fk_plucking_pluckingseesion1_idx");

            entity.HasIndex(e => e.UserId, "fk_plucking_user1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.LeaftypeId).HasColumnName("leaftype_id");
            entity.Property(e => e.PluckerId).HasColumnName("plucker_id");
            entity.Property(e => e.PluckingseesionId).HasColumnName("pluckingseesion_id");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Area).WithMany(p => p.Pluckings)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("fk_plucking_area1");

            entity.HasOne(d => d.Leaftype).WithMany(p => p.Pluckings)
                .HasForeignKey(d => d.LeaftypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_plucking_leaftype1");

            entity.HasOne(d => d.Plucker).WithMany(p => p.Pluckings)
                .HasForeignKey(d => d.PluckerId)
                .HasConstraintName("fk_plucking_employee1");

            entity.HasOne(d => d.Pluckingseesion).WithMany(p => p.Pluckings)
                .HasForeignKey(d => d.PluckingseesionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_plucking_pluckingseesion1");

            entity.HasOne(d => d.User).WithMany(p => p.Pluckings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_plucking_user1");
        });

        modelBuilder.Entity<Pluckingseesion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__plucking__3213E83F5D9E2559");

            entity.ToTable("pluckingseesion", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Privilage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__privilag__3213E83F6808F9D5");

            entity.ToTable("privilage", "uvateafactory");

            entity.HasIndex(e => e.ModuleId, "fk_privilage_module1_idx");

            entity.HasIndex(e => e.OperationId, "fk_privilage_operation1_idx");

            entity.HasIndex(e => e.RoleId, "fk_privilage_role1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Authority)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("authority");
            entity.Property(e => e.ModuleId).HasColumnName("module_id");
            entity.Property(e => e.OperationId).HasColumnName("operation_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Module).WithMany(p => p.Privilages)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_privilage_module1");

            entity.HasOne(d => d.Operation).WithMany(p => p.Privilages)
                .HasForeignKey(d => d.OperationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_privilage_operation1");

            entity.HasOne(d => d.Role).WithMany(p => p.Privilages)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_privilage_role1");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__product__3213E83FEE6B4EF5");

            entity.ToTable("product", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Qtyonhand)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("qtyonhand");
            entity.Property(e => e.Unitprice)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("unitprice");
        });

        modelBuilder.Entity<Production>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__producti__3213E83FAAC43166");

            entity.ToTable("production", "uvateafactory");

            entity.HasIndex(e => e.ProductionorderId, "fk_production_productionorder1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.ProductionorderId).HasColumnName("productionorder_id");
            entity.Property(e => e.Time).HasColumnName("time");

            entity.HasOne(d => d.Productionorder).WithMany(p => p.Productions)
                .HasForeignKey(d => d.ProductionorderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_production_productionorder1");
        });

        modelBuilder.Entity<Productionorder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__producti__3213E83F7773ABC3");

            entity.ToTable("productionorder", "uvateafactory");

            entity.HasIndex(e => e.AreaId, "fk_productionorder_area1_idx");

            entity.HasIndex(e => e.ProductionorderstatusId, "fk_productionorder_productionorderstatus1_idx");

            entity.HasIndex(e => e.UserId, "fk_productionorder_user1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Humidity)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("humidity");
            entity.Property(e => e.ProductionorderstatusId).HasColumnName("productionorderstatus_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Area).WithMany(p => p.Productionorders)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("fk_productionorder_area1");

            entity.HasOne(d => d.Productionorderstatus).WithMany(p => p.Productionorders)
                .HasForeignKey(d => d.ProductionorderstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productionorder_productionorderstatus1");

            entity.HasOne(d => d.User).WithMany(p => p.Productionorders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productionorder_user1");
        });

        modelBuilder.Entity<Productionorderstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__producti__3213E83F491D4190");

            entity.ToTable("productionorderstatus", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Productionproduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__producti__3213E83F05397797");

            entity.ToTable("productionproduct", "uvateafactory");

            entity.HasIndex(e => e.ProductId, "fk_production_has_product_product1_idx");

            entity.HasIndex(e => e.ProductionId, "fk_production_has_product_production1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductionId).HasColumnName("production_id");
            entity.Property(e => e.Quantity)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("quantity");

            entity.HasOne(d => d.Product).WithMany(p => p.Productionproducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_production_has_product_product1");

            entity.HasOne(d => d.Production).WithMany(p => p.Productionproducts)
                .HasForeignKey(d => d.ProductionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_production_has_product_production1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__role__3213E83FB7C34F19");

            entity.ToTable("role", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user__3213E83F05E84DB0");

            entity.ToTable("user", "uvateafactory");

            entity.HasIndex(e => e.EmployeeId, "fk_user_employee1_idx");

            entity.HasIndex(e => e.RoleId, "fk_user_role1_idx");

            entity.HasIndex(e => e.UserstatusId, "fk_user_userstatus1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Docreated).HasColumnName("docreated");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Tocreated).HasColumnName("tocreated");
            entity.Property(e => e.Username)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("username");
            entity.Property(e => e.UserstatusId).HasColumnName("userstatus_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.Users)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_employee1");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_role1");

            entity.HasOne(d => d.Userstatus).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_userstatus1");
        });

        modelBuilder.Entity<Userstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__userstat__3213E83FFA8F0201");

            entity.ToTable("userstatus", "uvateafactory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
