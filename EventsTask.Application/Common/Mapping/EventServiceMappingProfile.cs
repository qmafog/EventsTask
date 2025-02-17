using AutoMapper;
using EventsTask.Application.Common.Dtos;
using EventsTask.Domain.Entities;
using EventsTask.Domain.Models;


namespace EventsTask.Application.Common.Mapping
{
    public class EventServiceMappingProfile : Profile
    {
        public EventServiceMappingProfile()
        {
            CreateMap<Event, EventDto>();
            CreateMap<EventMember, EventMemberDto>();
        }
    }
}
