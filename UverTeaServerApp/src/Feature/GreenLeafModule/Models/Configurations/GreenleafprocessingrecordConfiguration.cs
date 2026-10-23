
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Models.Configurations;

public sealed class GreenleafprocessingrecordConfiguration
    : IEntityTypeConfiguration<Greenleafprocessingrecord>
{
    public void Configure(
        EntityTypeBuilder<Greenleafprocessingrecord> builder)
    {
        builder.ToTable("greenleafprocessingrecord", "uvateafactory");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.GreenLeafBatchId)
            .IsRequired();

        builder.Property(x => x.ProcessedWeight)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.ProcessingNotes)
            .HasMaxLength(1000);

        builder.Property(x => x.CompletedAt)
            .IsRequired();

        builder.HasIndex(x => x.GreenLeafBatchId)
            .IsUnique();

        builder.HasOne(x => x.GreenLeafBatch)
            .WithOne()
            .HasForeignKey<Greenleafprocessingrecord>(
                x => x.GreenLeafBatchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
