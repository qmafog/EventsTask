using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Domain.Entities
{
    public class EventMemberEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public DateTime? EventRegistrationDate { get; set; }
        public string? Email { get; set; }

        public EventEntity? Event { get; set; }
        public Guid EventId { get; set; }
    }
}
