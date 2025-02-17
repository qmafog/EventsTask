using EventsTask.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Interfaces
{
    public interface IRoleService
    {
        Task<HashSet<Role>> GetRolesAsync(Guid userId);

    }
}
