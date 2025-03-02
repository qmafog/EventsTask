using AutoMapper;
using EventsTask.Application.Common.Exceptions;
using EventsTask.Application.Common.Dtos;
using EventsTask.Application.Interfaces;
using EventsTask.Domain.Entities;
using EventsTask.Domain.Enums;
using EventsTask.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventsRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<Event> _eventValidator;

        public EventService(IEventsRepository eventsRepository, IMapper mapper, IValidator<Event> validator)
        {
            _mapper = mapper;
            _eventRepository = eventsRepository;
            _eventValidator = validator;
        }
        public async Task<Guid> CreateEvent(CreateEventDto createEventDto)
        {
            var eventModel = new Event
            {
                Id = Guid.NewGuid(),
                Title = createEventDto.Title,
                Description = createEventDto.Description,
                EventDate = createEventDto.EventDate,
                Location = createEventDto.Location,
                Category = createEventDto.Category,
                MaxMembers = createEventDto.MaxMembers,
                IsCompleted = false
            };
            var validationResult = await _eventValidator.ValidateAsync(eventModel);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var newId = await _eventRepository.AddAsync(eventModel);
            return newId;
        }

        public async Task DeleteEvent(Guid id)
        {
            var deletedId = await _eventRepository.DeleteAsync(id);
            if (deletedId is null)
                throw new NotFoundException(nameof(id), id);
        }

        public async Task<IEnumerable<EventDto>> GetAllEvents()
        {
            var events = await _eventRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task<IEnumerable<EventDto>> GetEventsByPage(int page, int pageSize)
        {
            var events = await _eventRepository.GetByPageAsync(page, pageSize);

            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task<EventDto?> GetEventById(Guid id)
        {
            var eventModel = await _eventRepository.GetByIdAsync(id);

            if (eventModel == null)
            {
                throw new NotFoundException(nameof(Event), id);
            }

            return _mapper.Map<EventDto>(eventModel);
        }

        public async Task<IEnumerable<EventDto>> GetEventsByTitle(string title)
        {
            var events = await _eventRepository.SearchByTitleAsync(title);
            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task<IEnumerable<EventDto>> GetEventsWithFilters(DateTime? date, string? location, EventCategory? category)
        {
            var events = await _eventRepository.FilterEventsAsync(date, location, category);
            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task UpdateEvent(UpdateEventDto updateEventDto)
        {
            var eventModel = new Event
            {
                Id = updateEventDto.Id,
                Title = updateEventDto.Title,
                Description = updateEventDto.Description,
                EventDate = updateEventDto.EventDate,
                Location = updateEventDto.Location,
                Category = updateEventDto.Category,
                MaxMembers = updateEventDto.MaxMembers,
                IsCompleted = updateEventDto.IsCompleted,
                Image = updateEventDto.Image
            };
            var validationResult = await _eventValidator.ValidateAsync(eventModel);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _eventRepository.UpdateAsync(eventModel);
        }

        public async Task<bool> UploadImage(Guid eventId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }
            using (var memoruStream = new MemoryStream())
            {
                await file.CopyToAsync(memoruStream);
                var fileBytes = memoruStream.ToArray();

                var eventModel = await _eventRepository.GetByIdAsync(eventId);
                if (eventModel == null)
                    return false;

                eventModel.Image = fileBytes;
                await _eventRepository
                    .UpdateAsync(eventModel);
            }

            return true;
        }
    }
}
