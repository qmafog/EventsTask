
using EventsTask.Domain.Enums;


namespace EventsTask.Domain.Models
{
    public class Event
    {    
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = null!;
        public EventCategory Category { get; set; }
        public int? MaxMembers { get; set; }
        public bool IsCompleted { get; set; } = false;
        public List<EventMember>? Members { get; set; }
        public byte[]? Image { get; set; }
    }
}

