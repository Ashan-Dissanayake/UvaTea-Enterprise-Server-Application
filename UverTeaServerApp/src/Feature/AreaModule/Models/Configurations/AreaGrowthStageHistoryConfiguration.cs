using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.AreaModule.Models.Configurations;

public class AreaGrowthStageHistoryConfiguration
    : IEntityTypeConfiguration<Areagrowthstagehistory>
{
    public void Configure(
        EntityTypeBuilder<Areagrowthstagehistory> builder)
    {
        builder.ToTable(
            "areagrowthstagehistory",
            schema: "uvateafactory");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(h => h.Area_id)
            .HasColumnName("area_id")
            .IsRequired();

        builder.Property(h => h.From_growthstage_id)
            .HasColumnName("from_growthstage_id")
            .IsRequired();

        builder.Property(h => h.To_growthstage_id)
            .HasColumnName("to_growthstage_id")
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
            .WithMany(a => a.AreaGrowthStageHistories)
            .HasForeignKey(h => h.Area_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.From_growthstage)
            .WithMany(g => g.Areagrowthstagehistoryfrom_growthstages)
            .HasForeignKey(h => h.From_growthstage_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.To_growthstage)
            .WithMany(g => g.Areagrowthstagehistoryto_growthstages)
            .HasForeignKey(h => h.To_growthstage_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.ChangedbyNavigation)
            .WithMany()
            .HasForeignKey(h => h.Changedby)
            .OnDelete(DeleteBehavior.Restrict);
    }
}