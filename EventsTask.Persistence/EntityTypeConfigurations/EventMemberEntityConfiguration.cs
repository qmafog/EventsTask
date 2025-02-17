using EventsTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Persistence.EntityTypeConfigurations
{
    public class EventMemberEntityConfiguration : IEntityTypeConfiguration<EventMemberEntity>
    {
        public void Configure(EntityTypeBuilder<EventMemberEntity> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.Surname)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(m => m.Email)
                .HasMaxLength(255);

            builder.Property(m => m.BirthDate)
                .HasColumnType("DATE");

            builder.Property(m => m.EventRegistrationDate)
                .HasColumnType("TIMESTAMP WITH TIME ZONE")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder
                .HasIndex(m => m.EventId);
        }
    }
}
