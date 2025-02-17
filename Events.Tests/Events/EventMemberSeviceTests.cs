using Events.Tests.Common;
using EventsTask.Application.Common.Dtos;
using EventsTask.Application.Common.Validators;
using EventsTask.Application.Services;
using EventsTask.Domain.Enums;
using EventsTask.Persistence;
using EventsTask.Persistence.Exceptions;
using EventsTask.Persistence.Repositories;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Tests.Events
{
    public class EventMemberSeviceTests : IClassFixture<EventsTestFixture>
    {
        private readonly EventMemberService _eventMemberService
            ;
        private readonly EventMembersRepository _repository;

        private readonly EventsDbContext _context;
        public EventMemberSeviceTests(EventsTestFixture fixture)
        {
            _context = fixture.Context;
            _repository = new EventMembersRepository(_context, fixture.Mapper);
            _eventMemberService = new EventMemberService(_repository, fixture.Mapper, new EventMemberValidator(), new RegisterEventMemberDtoValidator(new EventsRepository(_context, fixture.Mapper)));

        }

        [Fact]
        public async Task RegisterMember_ShouldAddMemberToEvent()
        {
            // Arrange
            var eventId = Guid.Parse("017C53B7-F5C7-415F-890C-F704897E85AF");
            var memberDto = new RegisterEventMemberDto
            {
                Name = "John",
                Surname = "Doe",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "johndoe@example.com",
                EventId = eventId
            };

            // Act
            var memberId = await _eventMemberService.RegisterMember(memberDto);
            var eventMembers = await _eventMemberService.GetEventMembers(eventId);

            // Assert
            eventMembers.ShouldContain(m => m.Id == memberId);
        }

        [Fact]
        public async Task GetEventMembers_ShouldReturnListOfMembers()
        {
            // Arrange
            var eventId = Guid.Parse("017C53B7-F5C7-415F-890C-F704897E85AF");

            // Act
            var members = await _eventMemberService.GetEventMembers(eventId);

            // Assert
            members.ShouldNotBeEmpty();
            members.Count().ShouldBe(2);
        }

        [Fact]
        public async Task GetMemberById_ShouldReturnCorrectMember()
        {
            // Arrange
            var eventId = Guid.Parse("017C53B7-F5C7-415F-890C-F704897E85AF");
            var memberDto = new RegisterEventMemberDto
            {
                Name = "Bob",
                Surname = "Brown",
                BirthDate = new DateTime(1988, 8, 8),
                Email = "bob@example.com",
                EventId = eventId
            };

            var memberId = await _eventMemberService.RegisterMember(memberDto);

            // Act
            var member = await _eventMemberService.GetMemberById(eventId, memberId);

            // Assert
            member.ShouldNotBeNull();
            member.Name.ShouldBe("Bob");
            member.Email.ShouldBe("bob@example.com");
        }

        [Fact]
        public async Task GetMemberById_ShouldReturnNull()
        {
            // Arrange
            var eventId = Guid.Parse("017C53B7-F5C7-415F-890C-F704897E85AF");
     

            // Act
            var member = await _eventMemberService.GetMemberById(eventId, Guid.Parse("017C53B7-F5C7-415F-890C-F704897E85AF"));

            // Assert
            member.ShouldBeNull();
        }

        [Fact]
        public async Task UnregisterMember_ShouldRemoveMemberFromEvent()
        {
            // Arrange
            var eventId = Guid.Parse("017C53B7-F5C7-415F-890C-F704897E85AF");
            var memberDto = new RegisterEventMemberDto
            {
                Name = "Charlie",
                Surname = "Davis",
                BirthDate = new DateTime(2000, 3, 15),
                Email = "charlie@example.com",
                EventId = eventId
            };

            var memberId = await _eventMemberService.RegisterMember(memberDto);

            // Act
            await _eventMemberService.UnregisterMember(eventId, memberId);
            var members = await _eventMemberService.GetEventMembers(eventId);

            // Assert
            members.ShouldNotContain(m => m.Id == memberId);
        }

        [Fact]
        public async Task UnregisterMember_ShouldThrowNotFoundException()
        {
            // Arrange
            var nonExistentEventId = Guid.NewGuid();
            var nonExistentMemberId = Guid.NewGuid();

            // Act & Assert
            await Should.ThrowAsync<NotFoundException>(() =>
                _eventMemberService.UnregisterMember(nonExistentEventId, nonExistentMemberId));
        }
    }
}
