using EventsTask.Application.Interfaces;
using EventsTask.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUserRepository _userRepository;
        public RoleService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<HashSet<Role>> GetRolesAsync(Guid userId)
        {
            return _userRepository.GetUserRoles(userId);
        }
    }
}
