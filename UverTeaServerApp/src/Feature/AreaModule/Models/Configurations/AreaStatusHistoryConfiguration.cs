using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.AreaModule.Models.Configurations;

public class AreaStatusHistoryConfiguration
    : IEntityTypeConfiguration<Areastatushistory>
{
    public void Configure(
        EntityTypeBuilder<Areastatushistory> builder)
    {
        builder.ToTable(
            "areastatushistory",
            schema: "uvateafactory");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(h => h.Area_id)
            .HasColumnName("area_id")
            .IsRequired();

        builder.Property(h => h.From_status_id)
            .HasColumnName("from_status_id")
            .IsRequired();

        builder.Property(h => h.To_status_id)
            .HasColumnName("to_status_id")
            .IsRequired();

        builder.Property(h => h.Changedat)
            .HasColumnName("changedat")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(h => h.Changedby)
            .HasColumnName("changedby")
            .IsRequired();

        builder.Property(h => h.Reason)
            .HasColumnName("reason")
            .HasMaxLength(255)
            .IsUnicode(false);

        builder.HasOne(h => h.Area)
            .WithMany(a => a.AreaStatusHistories)
            .HasForeignKey(h => h.Area_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.From_status)
            .WithMany()
            .HasForeignKey(h => h.From_status_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.To_status)
            .WithMany()
            .HasForeignKey(h => h.To_status_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.ChangedbyNavigation)
            .WithMany()
            .HasForeignKey(h => h.Changedby)
            .OnDelete(DeleteBehavior.Restrict);
    }
}