using EventsTask.Domain.Entities;
using EventsTask.Domain.Enums;
using EventsTask.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<Guid?> AddAsync(string username, string password);
        Task<User?> GetByUsername(string username);
        Task<UserEntity?> GetById(Guid id);
        Task<HashSet<Role>> GetUserRoles(Guid userId);
        void UpdateRefreshToken(Guid id, string refreshToken, DateTime? expires);
    }
}
