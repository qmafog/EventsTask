using AutoMapper;
using EventsTask.Domain.Entities;
using EventsTask.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Common.Mapping
{
    public class EventsMappingProfile : Profile
    {
        public EventsMappingProfile()
        {
            CreateMap<EventEntity, Event>()
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members));

            CreateMap<Event, EventEntity>()
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members));

            CreateMap<EventMemberEntity, EventMember>().ReverseMap();
        }
    }
}
