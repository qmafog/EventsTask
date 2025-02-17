using EventsTask.Domain.Entities;
using EventsTask.Domain.Enums;
using EventsTask.Domain.Models;


namespace EventsTask.Application.Interfaces
{
    public interface IEventsRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<IEnumerable<Event>> GetByPageAsync(int page, int pageSize);
        Task<Event?> GetByIdAsync(Guid id);
        Task<IEnumerable<Event>> SearchByTitleAsync(string title);
        Task<IEnumerable<Event>> FilterEventsAsync(DateTime? date, string? location, EventCategory? category);
        Task<Guid> AddAsync(Event eventModel);
        Task UpdateAsync(Event eventModel);
        Task DeleteAsync(Guid id);
    }
}
