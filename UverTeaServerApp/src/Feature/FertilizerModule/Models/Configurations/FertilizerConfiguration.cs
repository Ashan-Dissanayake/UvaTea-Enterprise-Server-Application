using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.FertilizerModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.FertilizerModule.Models.Configurations;

public class FertilizerConfiguration : IEntityTypeConfiguration<Fertilizer>
{
    public void Configure(EntityTypeBuilder<Fertilizer> builder)
    {
        builder.ToTable("fertilizer");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasColumnName("id");

        builder.Property(f => f.Name)
            .HasColumnName("name")
            .HasMaxLength(150);

        builder.Property(f => f.BrandId)
            .HasColumnName("brand_id")
            .IsRequired();

        builder.Property(f => f.FertilizertypeId)
            .HasColumnName("fertilizertype_id")
            .IsRequired();

        builder.Property(f => f.Quantity)
            .HasColumnName("quantity")
            .HasPrecision(18, 3);

        builder.Property(f => f.Unitprice)
            .HasColumnName("unitprice")
            .HasPrecision(18, 2);

        builder.Property(f => f.Rop)
            .HasColumnName("rop")
            .HasPrecision(18, 3);

        builder.Property(f => f.FertilizerstatusId)
            .HasColumnName("fertilizerstatus_id")
            .IsRequired();

        builder.Property(f => f.UnitOfMeasureId)
            .HasColumnName("unitofmeasure_id")
            .IsRequired();

        builder.Property(f => f.Dointroduced)
            .HasColumnName("dointroduced")
            .HasColumnType("date");

        builder.Property(f => f.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(f => f.Createdat)
            .HasColumnName("createdat")
            .IsRequired();

        builder.Property(f => f.Updatedat)
            .HasColumnName("updatedat");

        builder.Property(f => f.RowVersion)
            .HasColumnName("rowversion")
            .IsRowVersion();

        // Relationships

        builder.HasOne(f => f.Brand)
            .WithMany(b => b.Fertilizers)
            .HasForeignKey(f => f.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Fertilizertype)
            .WithMany(t => t.Fertilizers)
            .HasForeignKey(f => f.FertilizertypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Fertilizerstatus)
            .WithMany(s => s.Fertilizers)
            .HasForeignKey(f => f.FertilizerstatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.UnitOfMeasure)
            .WithMany(u => u.Fertilizers)
            .HasForeignKey(f => f.UnitOfMeasureId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.User)
            .WithMany(u => u.Fertilizers)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes

        builder.HasIndex(f => f.Name);

        builder.HasIndex(f => f.BrandId);

        builder.HasIndex(f => f.FertilizertypeId);

        builder.HasIndex(f => f.FertilizerstatusId);

        builder.HasIndex(f => f.UnitOfMeasureId);
    }
}