using EventsTask.Application.Interfaces;
using EventsTask.Application.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventsTask.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventMembersController : ControllerBase
    {
        private readonly IEventMemberService _memberService;

        public EventMembersController(IEventMemberService eventMemberService)
        {
            _memberService = eventMemberService;
        }

        /// <summary>
        /// Registers a participant for an event.
        /// </summary>
        /// <param name="eventId">The event ID.</param>
        /// <param name="memberDto">Participant details.</param>
        /// <returns>The registered participant ID.</returns>
        /// <response code="201">Participant successfully registered</response>
        /// <response code="400">Invalid request data</response>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/EventsMembers
        ///     {
        ///        "name": "John",
        ///        "surname": "Doe",
        ///        "birthDate": "1995-07-15",
        ///        "email": "john.doe@example.com"
        ///        "eventId": "b3a1a8e2-1234-5678-9012-abcdefabcdef"
        ///     }
        /// </remarks>
        [HttpPost]
        //[Authorize(Policy = "UserPolicy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterMember(Guid eventId, [FromBody] RegisterEventMemberDto memberDto)
        {
            var memberId = await _memberService.RegisterMember(memberDto);
            return CreatedAtAction(nameof(GetMemberById), new { eventId, memberId }, memberId);
        }

        /// <summary>
        /// Retrieves all participants of an event.
        /// </summary>
        /// <param name="eventId">The event ID.</param>
        /// <returns>List of participants.</returns>
        /// <response code="200">Returns the list of participants</response>
        [HttpGet]
        //[Authorize(Policy = "AdminPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EventMemberDto>))]
        public async Task<IActionResult> GetEventMembers(Guid eventId)
        {
            var members = await _memberService.GetEventMembers(eventId);
            return Ok(members);
        }

        /// <summary>
        /// Retrieves a participant by their ID.
        /// </summary>
        /// <param name="eventId">The event ID.</param>
        /// <param name="memberId">The participant ID.</param>
        /// <returns>Participant details.</returns>
        /// <response code="200">Participant found</response>
        /// <response code="404">Participant not found</response>
        [HttpGet("{memberId}")]
        //[Authorize(Policy = "AdminPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventMemberDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberById(Guid eventId, Guid memberId)
        {
            var member = await _memberService.GetMemberById(eventId, memberId);
            if (member == null) return NotFound();
            return Ok(member);
        }

        /// <summary>
        /// Removes a participant from an event.
        /// </summary>
        /// <param name="eventId">The event ID.</param>
        /// <param name="memberId">The participant ID.</param>
        /// <response code="204">Participant successfully removed</response>
        /// <response code="404">Participant not found</response>
        [HttpDelete("{memberId}")]
        //[Authorize(Policy = "UserPolicy")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnregisterMember(Guid eventId, Guid memberId)
        {
            await _memberService.UnregisterMember(eventId, memberId);
            return NoContent();
        }
    }
}
