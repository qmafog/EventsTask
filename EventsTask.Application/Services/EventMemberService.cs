using AutoMapper;
using EventsTask.Application.Common.Dtos;
using EventsTask.Application.Interfaces;
using EventsTask.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Services
{
    public class EventMemberService : IEventMemberService
    {
        private readonly IEventMembersRepository _eventMemberRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<EventMember> _validator;
        private readonly IValidator<RegisterEventMemberDto> _registerMemberValidator;

        public EventMemberService(IEventMembersRepository eventMemberService, 
            IMapper mapper, 
            IValidator<EventMember> validator,
            IValidator<RegisterEventMemberDto> registerMemberValidator)
        {
            _eventMemberRepository = eventMemberService;
            _mapper = mapper;
            _validator = validator;
            _registerMemberValidator = registerMemberValidator;
        }

        public async Task<IEnumerable<EventMemberDto>> GetEventMembers(Guid eventId)
        {
            var eventMembers = await _eventMemberRepository.GetByEventIdAsync(eventId);

            return _mapper.Map<List<EventMemberDto>>(eventMembers);
        }

        public async Task<IEnumerable<EventMemberDto>> GetEventMembersByPage(Guid eventId, int page, int pageSize)
        {
            var eventMembers = await _eventMemberRepository.GetByPageAsync(eventId, page, pageSize);

            return _mapper.Map<List<EventMemberDto>>(eventMembers);
        }

        public async Task<EventMemberDto?> GetMemberById(Guid eventId, Guid memberId)
        {
            var member = await _eventMemberRepository.GetByIdAsync(eventId, memberId);

            return member != null ? _mapper.Map<EventMemberDto>(member) : null;
        }

        public async Task<Guid> RegisterMember(RegisterEventMemberDto memberDto)
        {
            var eventId = memberDto.EventId;
            var validationResult = await _registerMemberValidator.ValidateAsync(memberDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var eventMember = new EventMember
            {
                Id = Guid.NewGuid(),
                Name = memberDto.Name,
                Surname = memberDto.Surname,
                BirthDate = memberDto.BirthDate,
                Email = memberDto.Email,
                EventId = eventId
            };
            validationResult = await _validator.ValidateAsync(eventMember);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var result = await _eventMemberRepository.AddAsync(eventMember);
            return result;
        }

        public async Task UnregisterMember(Guid eventId, Guid memberId)
        {
            await _eventMemberRepository.RemoveAsync(eventId, memberId);
        }
    }
}
