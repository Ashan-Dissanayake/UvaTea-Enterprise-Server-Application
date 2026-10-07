using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Models.Configurations;

public class LeafbatchstatusConfiguration : IEntityTypeConfiguration<Leafbatchstatus>
{
    public void Configure(EntityTypeBuilder<Leafbatchstatus> builder)
    {
        // Table Name
        builder.ToTable("leafbatchstatus", schema: "uvateafactory");

        // Primary Key
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        // Columns & Constraints
        builder.Property(s => s.Name)
               .HasColumnName("name")
               .HasMaxLength(45)
               .IsRequired();
    }
}