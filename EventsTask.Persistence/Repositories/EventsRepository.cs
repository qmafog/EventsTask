using AutoMapper;
using EventsTask.Application.Interfaces;
using EventsTask.Domain.Entities;
using EventsTask.Domain.Enums;
using EventsTask.Domain.Models;
using EventsTask.Persistence.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Persistence.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly EventsDbContext _dbContext;
        private readonly IMapper _mapper;

        public EventsRepository(EventsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
           

        public async Task<Guid> AddAsync(Event eventModel)
        {
            var eventEntity = _mapper.Map<EventEntity>(eventModel);
            var result = await _dbContext.Events.AddAsync(eventEntity);
            await _dbContext.SaveChangesAsync();

            return result.Entity.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var eventEntity = await _dbContext.Events.FindAsync(id);
     
            if (eventEntity == null)
            {
                throw new NotFoundException(nameof(EventEntity), id);
            }

            _dbContext.Events.Remove(eventEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Event>> FilterEventsAsync(DateTime? date, string? location, EventCategory? category)
        {
            var query = _dbContext.Events.AsQueryable();

            if (date.HasValue)
                query = query.Where(e => e.EventDate == date.Value);

            if (!string.IsNullOrEmpty(location))
                query = query.Where(e => e.Location == location);

            if (category.HasValue)
                query = query.Where(e => e.Category == category.Value);

            var eventEntities = await query.ToListAsync();

            var events = _mapper.Map<List<Event>>(eventEntities);

            return events;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            var eventEntities = await _dbContext.Events.ToListAsync();
            var events = _mapper.Map<List<Event>>(eventEntities);

            return events;
        }

        public async Task<Event?> GetByIdAsync(Guid id)
        {
            var eventEntity = await _dbContext.Events
                .Include(e => e.Members)
                .FirstOrDefaultAsync(e => e.Id == id);

            return eventEntity != null ? _mapper.Map<Event>(eventEntity) : null;
        }

        public async Task<IEnumerable<Event>> SearchByTitleAsync(string title)
        {
            var eventEntities = await _dbContext.Events
                .Where(e => EF.Functions.ILike(e.Title, $"%{title}%"))
                .ToListAsync();

            return _mapper.Map<List<Event>>(eventEntities);
        }

        public async Task UpdateAsync(Event eventModel)
        {
            var eventEntity = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == eventModel.Id);

            if (eventEntity == null)
            {
                throw new NotFoundException(nameof(EventEntity), eventModel.Id);
            }

            _mapper.Map(eventModel, eventEntity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Event>> GetByPageAsync(int page, int pageSize)
        {
            var eventEntities = await _dbContext.Events.Include(e => e.Members)
                                    .AsNoTracking()
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
            var events = _mapper.Map<List<Event>>(eventEntities);

            return events;
        }
    }
}
