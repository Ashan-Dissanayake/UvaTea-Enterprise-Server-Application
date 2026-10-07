using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Models.Configurations;

public class GreenleafbatchConfiguration : IEntityTypeConfiguration<Greenleafbatch>
{
       public void Configure(EntityTypeBuilder<Greenleafbatch> builder)
       {
              // Table Name
              builder.ToTable("greenleafbatch", schema: "uvateafactory");

              // Primary Key
              builder.HasKey(g => g.Id);

              builder.Property(g => g.Id)
                     .HasColumnName("id")
                     .ValueGeneratedOnAdd();

              // Columns & Constraints
              builder.Property(g => g.BatchNumber)
                     .HasColumnName("batchnumber")
                     .HasMaxLength(45)
                     .IsRequired();

              builder.HasIndex(g => g.BatchNumber)
                     .IsUnique();

              builder.Property(g => g.AreaId)
                     .HasColumnName("area_id")
                     .IsRequired();

              builder.Property(g => g.ContainerTypeId)
                     .HasColumnName("containertype_id")
                     .IsRequired();

              builder.Property(g => g.WeatherConditionId)
                     .HasColumnName("weathercondition_id")
                     .IsRequired();

              builder.Property(g => g.LeafConditionId)
                     .HasColumnName("leafcondition_id")
                     .IsRequired();

              builder.Property(g => g.LeafBatchStatusId)
                     .HasColumnName("leafbatch_status_id")
                     .IsRequired();

              builder.Property(g => g.PluckingDateTime)
                     .HasColumnName("pluckingdatetime")
                     .HasColumnType("datetime")
                     .IsRequired();

              builder.Property(g => g.CollectionDateTime)
                     .HasColumnName("collectiondatetime")
                     .HasColumnType("datetime")
                     .IsRequired();

              builder.Property(g => g.ArrivalDateTime)
                     .HasColumnName("arrivaldatetime")
                     .HasColumnType("datetime")
                     .IsRequired();

              builder.Property(g => g.Weight)
                     .HasColumnName("weight")
                     .HasColumnType("decimal(10,2)")
                     .IsRequired();

              builder.Property(g => g.Remarks)
                     .HasColumnName("remarks")
                     .HasColumnType("text");

              builder.Property(g => g.CreatedAt)
                     .HasColumnName("created_at")
                     .HasColumnType("datetime")
                     .IsRequired();

              builder.Property(g => g.UpdatedAt)
                     .HasColumnName("updated_at")
                     .HasColumnType("datetime")
                     .IsRequired();

              // Relationships
              builder.HasOne(g => g.Area)
                     .WithMany()
                     .HasForeignKey(g => g.AreaId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(g => g.ContainerType)
                     .WithMany(c => c.Greenleafbatchs)
                     .HasForeignKey(g => g.ContainerTypeId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(g => g.WeatherCondition)
                     .WithMany(w => w.Greenleafbatchs)
                     .HasForeignKey(g => g.WeatherConditionId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(g => g.LeafCondition)
                     .WithMany(l => l.Greenleafbatchs)
                     .HasForeignKey(g => g.LeafConditionId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(g => g.LeafBatchStatus)
                     .WithMany(s => s.Greenleafbatchs)
                     .HasForeignKey(g => g.LeafBatchStatusId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(r => r.GreenLeafBatch)
                     .WithOne(g => g.GreenLeafBatch)
                     .HasForeignKey<Greenleafriskassessment>( r => r.GreenLeafBatchId);
                     
               // One-to-One Relationship
              builder.HasOne(r => r.GreenLeafBatch)
                     .WithOne(g => g.GreenLeafBatch)
                     .HasForeignKey<Greenleafriskassessment>( r => r.GreenLeafBatchId)
                     .OnDelete(DeleteBehavior.Restrict);

              // Unique Constraint
              builder.HasIndex(r => r.GreenLeafBatch)
                     .IsUnique();

       }
}