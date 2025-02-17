using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsTask.Domain.Entities;
using EventsTask.Domain.Models;
using FluentValidation;

namespace EventsTask.Application.Common.Validators
{
    public class EventMemberValidator : AbstractValidator<EventMember>
    {
        public EventMemberValidator()
        {
            RuleFor(m => m.BirthDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Некорретное значение даты!");

            RuleFor(m => m.EventId)
                .NotEmpty().WithMessage("Участник не может быть вне события!");
        }
    }
}
