using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Models.Configurations;

public class GreenleafriskassessmentConfiguration
    : IEntityTypeConfiguration<Greenleafriskassessment>
{
    public void Configure(EntityTypeBuilder<Greenleafriskassessment> builder)
    {
        // Table
        builder.ToTable(
            "greenleafriskassessment",
            schema: "uvateafactory");

        // Primary Key
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // Foreign Key
        builder.Property(r => r.GreenLeafBatchId)
            .HasColumnName("greenleafbatch_id")
            .IsRequired();

        // Risk Assessment Properties
        builder.Property(r => r.RiskScore)
            .HasColumnName("riskscore")
            .IsRequired();

        builder.Property(r => r.RiskLevel)
            .HasColumnName("risklevel")
            .HasMaxLength(45)
            .IsRequired();

        builder.Property(r => r.RiskReason)
            .HasColumnName("riskreason")
            .HasColumnType("text");

        builder.Property(r => r.AssessedAt)
            .HasColumnName("assessed_at")
            .HasColumnType("datetime")
            .IsRequired();

        // One-to-One Relationship
        builder.HasOne(r => r.GreenLeafBatch)
            .WithOne(g => g.RiskAssessment)
            .HasForeignKey<Greenleafriskassessment>(
                r => r.GreenLeafBatchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique Foreign Key
        builder.HasIndex(r => r.GreenLeafBatchId)
            .IsUnique();
    }
}