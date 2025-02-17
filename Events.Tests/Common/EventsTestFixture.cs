using AutoMapper;
using EventsTask.Application.Common.Mapping;
using EventsTask.Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Tests.Common
{
    public class EventsTestFixture : IDisposable
    {
        public EventsDbContext Context;
        public IMapper Mapper;

        public EventsTestFixture()
        {
            Context = EventsContextFactory.Create();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EventServiceMappingProfile());
                cfg.AddProfile(new EventsMappingProfile());
            });
            Mapper = config.CreateMapper();
        }

        public void Dispose()
        {
            EventsContextFactory.Destroy(Context);
        }


    }
    [CollectionDefinition("GetsCollection")]
    public class GetsCollection : ICollectionFixture<EventsTestFixture> { }
}
