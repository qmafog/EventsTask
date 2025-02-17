using EventsTask.Domain.Entities;
using EventsTask.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Common.Validators
{
    public class EventValidator : AbstractValidator<Event>
    {
        public EventValidator()
        {
            RuleFor(e => e.MaxMembers)
                .GreaterThan(0).WithMessage("Невозможно задать данное значение!");

            RuleFor(e => e.Members)
                .Must((e, members) => members == null || members.Count <= e.MaxMembers)
                .WithMessage("Максимальное количестов участников!");

            RuleFor(e => e.IsCompleted)
                .Equal(true)
                .When(e => e.EventDate < DateTime.UtcNow)
                .WithMessage("Невозможно для прошедшего события!");
        }
    }
    
}
