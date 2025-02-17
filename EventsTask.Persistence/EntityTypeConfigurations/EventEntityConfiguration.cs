using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EventsTask.Persistence.EntityTypeConfigurations
{
    public class EventEntityConfiguration : IEntityTypeConfiguration<EventEntity>
    {

        public void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.Category)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.MaxMembers)
                .IsRequired();

            builder.Property(e => e.Image)
                .IsRequired(false)
                .HasColumnType("BYTEA");

            builder.HasMany(e => e.Members)
                .WithOne(m => m.Event)
                .HasForeignKey(m => m.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.EventDate);
            builder.HasIndex(e => e.Category);
            builder.HasIndex(e => e.Location);
            builder.HasIndex(e => e.Title);
        }
    }
}
