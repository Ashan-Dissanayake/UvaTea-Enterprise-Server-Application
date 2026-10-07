using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Models.Configurations;

public class GreenleafriskassessmentConfiguration
    : IEntityTypeConfiguration<Greenleafriskassessment>
{
    public void Configure(EntityTypeBuilder<Greenleafriskassessment> builder)
    {
        // Table Name
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

        // Risk Score
        builder.Property(r => r.RiskScore)
               .HasColumnName("riskscore")
               .IsRequired();

        // Risk Level
        builder.Property(r => r.RiskLevel)
               .HasColumnName("risklevel")
               .HasMaxLength(45)
               .IsRequired();

        // Risk Reason
        builder.Property(r => r.RiskReason)
               .HasColumnName("riskreason")
               .HasColumnType("text");

        // Assessed At
        builder.Property(r => r.AssessedAt)
               .HasColumnName("assessed_at")
               .HasColumnType("datetime")
               .IsRequired();

        // =========================================================
        // One-to-One Relationship
        // =========================================================

        builder.HasOne(r => r.GreenLeafBatch)
               .WithOne(g => g.GreenLeafBatch)
               .HasForeignKey<Greenleafriskassessment>(
                   r => r.GreenLeafBatchId)
               .OnDelete(DeleteBehavior.Cascade);

        // Ensure One-to-One
        builder.HasIndex(r => r.GreenLeafBatchId)
               .IsUnique();
    }
}