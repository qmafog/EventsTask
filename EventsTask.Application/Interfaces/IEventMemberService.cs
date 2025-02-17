using EventsTask.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EventsTask.Application.Interfaces
{
    public interface IEventMemberService
    {

        Task<Guid> RegisterMember(RegisterEventMemberDto memberDto);
        Task<IEnumerable<EventMemberDto>> GetEventMembers(Guid eventId);
        Task<IEnumerable<EventMemberDto>> GetEventMembersByPage(Guid eventId, int page, int pageSize);
        Task<EventMemberDto?> GetMemberById(Guid eventId, Guid memberId);
        Task UnregisterMember(Guid eventId, Guid memberId);
    }
}
