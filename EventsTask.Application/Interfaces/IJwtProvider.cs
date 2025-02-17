using EventsTask.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user, bool isAdmin);
    }
}
