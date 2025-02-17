using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsTask.Application.Interfaces
{
    public interface IEventsDbContext
    {
        DbSet<EventEntity> Events { get; set; }
        DbSet<EventMemberEntity> EventMembers { get; set; }

 
    }
}
