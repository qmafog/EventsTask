using EventsTask.Application.Interfaces;
using EventsTask.Domain.Entities;
using EventsTask.Domain.Enums;
using EventsTask.Domain.Models;
using EventsTask.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EventsDbContext _dbContext;
        public UserRepository(EventsDbContext context)
        {
            _dbContext = context;
        }
        public async Task<Guid?> AddAsync(string username, string password)
        {
            var role = await _dbContext.Roles.FindAsync(new object[] { (int)(Role.User) });

            if (role is null)
            {
                return null;
            }

            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                UserName = username,
                PasswordHash = password,
                Roles = new List<RoleEntity> { role }
            };
            var result = await _dbContext.Users.AddAsync(userEntity);
            var userId = result.Entity.Id;

            await _dbContext.SaveChangesAsync();
            return userId;
        }

        public async Task<User?> GetByUsername(string username)
        {
            var userEntity = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (userEntity == null)
            {
                return null;
            }

            return new User
            {
                Id = userEntity.Id,
                UserName = userEntity.UserName,
                PasswordHash = userEntity.PasswordHash
            };
            
            
        }

        public async Task<HashSet<Role>> GetUserRoles(Guid userId)
        {
            var roleEntities = await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Roles)
                .ToListAsync();

            return roleEntities
                    .Select(r => (Role)r.Id)
                    .ToHashSet();
        }
    }
}
