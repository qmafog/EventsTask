using EventsTask.Application.Common.Dtos;
using EventsTask.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Common.Validators
{
    public class RegisterEventMemberDtoValidator : AbstractValidator<RegisterEventMemberDto>
    {
        private readonly IEventsRepository _eventRepository;

        public RegisterEventMemberDtoValidator(IEventsRepository eventRepository)
        {
            _eventRepository = eventRepository;

            RuleFor(x => x.EventId)
                .MustAsync(EventHasSpace)
                .WithMessage("На событие зарегистрировано максимальное количество участников.");
        }

        private async Task<bool> EventHasSpace(Guid eventId, CancellationToken cancellationToken)
        {
            var eventModel = await _eventRepository.GetByIdAsync(eventId);
            if (eventModel == null) return false;

            return eventModel.Members.Count < eventModel.MaxMembers;
        }
    }
}
