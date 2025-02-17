using EventsTask.Domain.Entities;
using EventsTask.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Interfaces
{
    public interface IEventMembersRepository
    {
        Task<IEnumerable<EventMember>> GetByEventIdAsync(Guid eventId);
        Task<IEnumerable<EventMember>> GetByPageAsync(Guid eventId, int page, int pageSize);
        Task<EventMember?> GetByIdAsync(Guid eventId, Guid id);
        Task<Guid> AddAsync(EventMember eventMember);
        Task RemoveAsync(Guid eventId, Guid id);
    }
}
