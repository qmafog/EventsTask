using EventsTask.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Persistence.EntityTypeConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {

        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.Roles)
                    .WithMany(r => r.Users)
                    .UsingEntity(j => j.ToTable("UserRoles"));

         
        }
    }
}
