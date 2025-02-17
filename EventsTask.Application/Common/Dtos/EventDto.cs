using EventsTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsTask.Domain.Enums;

namespace EventsTask.Application.Common.Dtos
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = null!;
        public EventCategory Category { get; set; }
        public int? MaxMembers { get; set; }
        public byte[]? Image { get; set; }
    }
}
