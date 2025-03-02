using Events.Tests.Common;
using EventsTask.Application.Services;
using EventsTask.Persistence.Repositories;
using EventsTask.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsTask.Application.Common.Dtos;
using EventsTask.Domain.Enums;
using Shouldly;
using EventsTask.Application.Common.Validators;
using EventsTask.Application.Common.Exceptions;

namespace Events.Tests.Events
{
    public class EventServiceTests : IClassFixture<EventsTestFixture>
    {
        private readonly EventService _eventService;

        private readonly EventsRepository _repository;

        private readonly EventsDbContext _context;
        public EventServiceTests(EventsTestFixture fixture)
        {
            _context = fixture.Context;
            _repository = new EventsRepository(_context, fixture.Mapper);
            _eventService = new EventService(_repository, fixture.Mapper, new EventValidator());
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnEvents()
        {
            // Act
            var result = await _eventService.GetAllEvents();

            // Assert
            result.ShouldNotBeEmpty();
            result.Count().ShouldBe(3);
           
        }

        [Fact]
        public async Task GetEventById_ShouldReturnCorrectEvent()
        {
            // Arrange
            var eventEntity = _context.Events.First();

            // Act
            var result = await _eventService.GetEventById(eventEntity.Id);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(eventEntity.Id);
            result.Title.ShouldBe(eventEntity.Title);
  
        }

        [Fact]
        public async Task GetEventById_ShouldThrowNotFoundException()
        {

            // Act & Assert
            await Should.ThrowAsync<NotFoundException>(() => _eventService.GetEventById(Guid.Parse("6C13F141-9FBC-4DED-9D54-AC3040F33075")));
      
   
        }

        [Fact]
        public async Task CreateEvent_ShouldAddEvent()
        {
            // Arrange
            var createEventDto = new CreateEventDto
            {
                Title = "New Event",
                Description = "New Event Description",
                EventDate = DateTime.UtcNow.AddDays(10),
                Location = "Berlin",
                Category = EventCategory.Conference,
                MaxMembers = 50
            };

            // Act
            var newEventId = await _eventService.CreateEvent(createEventDto);
            var addedEvent = await _context.Events.FindAsync(newEventId);

            // Assert
            addedEvent.ShouldNotBeNull();
            addedEvent.Title.ShouldBe(createEventDto.Title);
        }

        [Fact]
        public async Task DeleteEvent_ShouldRemoveEvent()
        {
            // Arrange
            var eventEntity = _context.Events.First();

            // Act
            await _eventService.DeleteEvent(eventEntity.Id);
            var deletedEvent = await _context.Events.FindAsync(eventEntity.Id);

            // Assert
            deletedEvent.ShouldBeNull();
        }

        [Fact]
        public async Task GetEventsWithFilters_ShouldReturnFilteredEvents()
        {
            // Act
            var result = await _eventService.GetEventsWithFilters(null, "Berlin", null);

            // Assert
            result.ShouldNotBeEmpty();
            result.All(e => e.Location == "Berlin").ShouldBeTrue();
        }

        [Fact]
        public async Task UpdateEvent_ShouldNotUpdate()
        {
            // Arrange
            var updateEventDto = new UpdateEventDto
            {
                Id = Guid.NewGuid(),
                Title = "Updated Title",
                Description = "Updated Description",
                EventDate = DateTime.UtcNow.AddDays(10),
                Location = "New Location",
                Category = EventCategory.Conference,
                MaxMembers = 100,
                IsCompleted = true
            };

            // Act
            await _eventService.UpdateEvent(updateEventDto);
            var attemptedUpdateEvent = await _context.Events.FindAsync(updateEventDto.Id);

            // Assert
            attemptedUpdateEvent.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteEvent_ShouldThrowNotFoundException()
        {
            // Arrange
            var nonExistentEventId = Guid.NewGuid();

            // Act & Assert
            await Should.ThrowAsync<NotFoundException>(() => _eventService.DeleteEvent(nonExistentEventId));
        }
    }
}
