using AutoMapper;
using EventsTask.Application.Interfaces;
using EventsTask.Domain.Entities;
using EventsTask.Domain.Models;
using EventsTask.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Persistence.Repositories
{
    public class EventMembersRepository : IEventMembersRepository
    {
        private readonly EventsDbContext _dbContext;
        private readonly IMapper _mapper;

        public EventMembersRepository(EventsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
           

        public async Task<Guid> AddAsync(EventMember eventMember)
        {
            var eventMemberEntity = _mapper.Map<EventMemberEntity>(eventMember);
            var result = await _dbContext.EventMembers.AddAsync(eventMemberEntity);
            await _dbContext.SaveChangesAsync();

            return result.Entity.Id;
        }

        public async Task<IEnumerable<EventMember>> GetByEventIdAsync(Guid eventId)
        {
            var eventMemberEntities = await _dbContext.EventMembers
                .Where(m => m.EventId == eventId)
                .ToListAsync();
            var eventMembers = _mapper.Map<List<EventMember>>(eventMemberEntities);

            return eventMembers;
        }
        public async Task<IEnumerable<EventMember>> GetByPageAsync(Guid eventId, int page, int pageSize)
        {
            var eventMemberEntities = await _dbContext.EventMembers
                .Where(m => m.EventId == eventId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var eventMembers = _mapper.Map<List<EventMember>>(eventMemberEntities);

            return eventMembers;
        }

        public async Task<EventMember?> GetByIdAsync(Guid eventId, Guid id)
        {
            var eventMemberEntity = await _dbContext.EventMembers.FirstOrDefaultAsync(m => m.Id == id && m.EventId == eventId);
            return eventMemberEntity != null ? _mapper.Map<EventMember>(eventMemberEntity) : null;
    
        }

        public async Task<Guid?> RemoveAsync(Guid eventId, Guid id)
        {
            var eventMemberEntity = await _dbContext.EventMembers.FindAsync(new object[] { id });

            if (eventMemberEntity == null)
            {
                return null;
            }

            var result = _dbContext.EventMembers.Remove(eventMemberEntity);
            await _dbContext.SaveChangesAsync();
            return result.Entity.Id;
        }
    }
}
