using EventsTask.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsTask.Domain.Entities;
using EventsTask.Domain.Enums;


namespace Events.Tests.Common
{
    public static class EventsContextFactory
    {
        public static EventsDbContext Create()
        {
            var options = new DbContextOptionsBuilder<EventsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new EventsDbContext(options);

            context.Database.EnsureCreated();
            context.Events.AddRange(
                new EventEntity
                {
                    Id = Guid.Parse("017C53B7-F5C7-415F-890C-F704897E85AF"),
                    Title = "Title 1",
                    Description = "Description 1",
                    EventDate = DateTime.Now,
                    Location = "Location 1",
                    Category = EventCategory.Conference,
                    MaxMembers = 5
                },
                new EventEntity
                {
                    Id = Guid.Parse("D51D1FBF-9596-4FF7-96A7-D5942A2558CA"),
                    Title = "Title 2",
                    Description = "Description 2",
                    EventDate = DateTime.Now,
                    Location = "Location 2",
                    Category = EventCategory.Meetup,
                    MaxMembers = 2
                }, new EventEntity
                {
                    Id = Guid.Parse("34A1ADDF-668D-4755-A92D-2767AF26DBAF"),
                    Title = "Title 3",
                    Description = "Description 3",
                    EventDate = DateTime.Now,
                    Location = "Location 3",
                    Category = EventCategory.Other,
                    MaxMembers = 3
                }
                );
            context.EventMembers.AddRange(
                new EventMemberEntity
                {
                    Id = Guid.Parse("6C13F141-9FBC-4DED-9D54-AC3040F33075"),
                    Name = "Name 1",
                    Surname = "Surname 1",
                    BirthDate = new DateTime(2001, 7, 1),
                    EventRegistrationDate = DateTime.Now,
                    Email = "Some email 1",
                    EventId = Guid.Parse("D51D1FBF-9596-4FF7-96A7-D5942A2558CA")
                },
                new EventMemberEntity
                {
                    Id = Guid.Parse("4954880D-2FA3-49E2-9556-CB07E9094F91"),
                    Name = "Name 2",
                    Surname = "Surname 2",
                    BirthDate = new DateTime(2002, 7, 2),
                    EventRegistrationDate = DateTime.Now,
                    Email = "Some email 2",
                    EventId = Guid.Parse("D51D1FBF-9596-4FF7-96A7-D5942A2558CA")
                },
                new EventMemberEntity
                {
                    Id = Guid.Parse("0CA21DDB-DFF4-4977-BFB1-F17FF98CBEB8"),
                    Name = "Name 3",
                    Surname = "Surname 3",
                    BirthDate = new DateTime(2003, 7, 3),
                    EventRegistrationDate = DateTime.Now,
                    Email = "Some email 3",
                    EventId = Guid.Parse("34A1ADDF-668D-4755-A92D-2767AF26DBAF")
                },
                new EventMemberEntity
                {
                    Id = Guid.Parse("D40E7E7C-5718-4570-BB4D-8792749EE8A4"),
                    Name = "Name 4",
                    Surname = "Surname 4",
                    BirthDate = new DateTime(2004, 7, 4),
                    EventRegistrationDate = DateTime.Now,
                    Email = "Some email 4",
                    EventId = Guid.Parse("34A1ADDF-668D-4755-A92D-2767AF26DBAF")
                }
                );
            context.SaveChangesAsync();
            return context;
        }

        public static void Destroy(EventsDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
