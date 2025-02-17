using EventsTask.Domain.Entities;
using Microsoft.AspNetCore.Http;
using EventsTask.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsTask.Domain.Enums;

namespace EventsTask.Application.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> GetAllEvents();
        Task<IEnumerable<EventDto>> GetEventsByPage(int page, int pageSize);
        Task<EventDto?> GetEventById(Guid id);
        Task<IEnumerable<EventDto>> GetEventsByTitle(string title);
        Task<Guid> CreateEvent(CreateEventDto createEventDto);
        Task UpdateEvent(UpdateEventDto updateEventDto);
        Task DeleteEvent(Guid id);
        Task<IEnumerable<EventDto>> GetEventsWithFilters(DateTime? date, string? location, EventCategory? category);
        Task<bool> UploadImage(Guid eventId, IFormFile file);
    }
}
