using EventsTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Common.Dtos
{
    public class EventMemberDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public DateTime EventRegistrationDate { get; set; }
        public string? Email { get; set; }
    }
}
