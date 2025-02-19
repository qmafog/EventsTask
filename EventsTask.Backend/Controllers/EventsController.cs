using EventsTask.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventsTask.Application.Common.Dtos;
using EventsTask.Domain.Enums;
using EventsTask.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace EventsTask.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IUsersService _usersService;

        public EventsController(IEventService eventService,
                                IUsersService usersService)
        {
            _eventService = eventService;
            _usersService = usersService;
        }

        /// <summary>
        /// Retrieves a list of all events.
        /// </summary>
        /// <returns>A list of events.</returns>
        /// <response code="200">Returns the list of events</response>
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EventDto>))]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventService.GetAllEvents();
            return Ok(events);
        }

        /// <summary>
        /// Retrieves an event by its ID.
        /// </summary>
        /// <param name="id">The event ID.</param>
        /// <returns>The event details.</returns>
        /// <response code="200">Event found</response>
        /// <response code="404">Event not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var eventDto = await _eventService.GetEventById(id);
            if (eventDto == null) return NotFound();
            return Ok(eventDto);
        }

        /// <summary>
        /// Retrieves an event by its title.
        /// </summary>
        /// <param name="title">The event title.</param>
        /// <returns>A list of events.</returns>
        /// <response code="200">Event found</response>
      
        [HttpGet("bytitle")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDto))]
        public async Task<IActionResult> GetEventsByTitle(string title)
        {
            var events = await _eventService.GetEventsByTitle(title);
            return Ok(events);
        }

        /// <summary>
        /// Retrieves a list of events by page.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">PageSize.</param>
        /// <returns>A list of events.</returns>
        /// <response code="200">Event found</response>

        [HttpGet("bypage")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDto))]
        public async Task<IActionResult> GetEventsByPage(int page, int pageSize)
        {
            var events = await _eventService.GetEventsByPage(page, pageSize);
            return Ok(events);
        }


        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="createEventDto">Event details.</param>
        /// <returns>The ID of the created event.</returns>
        /// <response code="201">Event successfully created</response>
        /// <response code="400">Invalid request data</response>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/events
        ///     {
        ///        "title": "Developer Conference",
        ///        "description": "A technical conference",
        ///        "eventDate": "2025-06-20T14:00:00Z",
        ///        "location": "New York",
        ///        "category": 1,
        ///        "maxMembers": 100
        ///     }
        /// </remarks>
        [HttpPost]
        //[Authorize(Policy = "AdminPolicy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto createEventDto)
        {
            var newEventId = await _eventService.CreateEvent(createEventDto);
            return CreatedAtAction(nameof(GetEventById), new { id = newEventId }, newEventId);
        }

        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="updateEventDto">Updated event data.</param>
        /// <response code="204">Event successfully updated</response>
        /// <response code="400">Invalid request data</response>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/events
        ///     {
        ///        "id": "b3a1a8e2-1234-5678-9012-abcdefabcdef",
        ///        "title": "Updated Conference",
        ///        "description": "Updated description",
        ///        "eventDate": "2025-06-21T14:00:00Z",
        ///        "location": "Los Angeles",
        ///        "category": 2,
        ///        "maxMembers": 150
        ///     }
        /// </remarks>
        [HttpPut]
        //[Authorize(Policy = "AdminPolicy")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventDto updateEventDto)
        {
            await _eventService.UpdateEvent(updateEventDto);
            return NoContent();
        }

        /// <summary>
        /// Deletes an event.
        /// </summary>
        /// <param name="id">The event ID.</param>
        /// <response code="204">Event successfully deleted</response>
        /// <response code="404">Event not found</response>
        [HttpDelete("{id}")]
        //[Authorize(Policy = "AdminPolicy")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            await _eventService.DeleteEvent(id);
            return NoContent();
        }

        /// <summary>
        /// Uploads image of an event.
        /// </summary>
        /// <param name="id">The event ID.</param>
        /// <param name="file">The uploaded image.</param>
        /// <response code="204">Event successfully updated</response>
        /// <responde code="400">Error uploading image</responde>
        [HttpPost("uploading-image")]
        //[Authorize(Policy = "AdminPolicy")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadEventImage(Guid id, IFormFile file)
        {
            var success = await _eventService.UploadImage(id, file);
            if (!success)
                return BadRequest("Empty Image");

            return NoContent();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var token = await _usersService.Login(username, password);
            Response.Cookies.Append("token", token);
            return Ok(token);
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            await _usersService.Register(username, password);


            return Ok();
        }
    }
}

