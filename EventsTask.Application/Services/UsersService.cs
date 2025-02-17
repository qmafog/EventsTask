using EventsTask.Application.Interfaces;
using EventsTask.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UsersService(IUserRepository userRepository,
                            IPasswordHasher passwordHasher,
                            IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task Register(string userName, string password)
        {
            var passwordHash = _passwordHasher.Generate(password);

            await _userRepository.AddAsync(userName, passwordHash);
        }

        public async Task<string> Login(string userName, string password)
        {
            var user = await _userRepository.GetByUsername(userName);

            if (user == null)
            {
                Console.WriteLine($"User {userName} not found!");
            }
            var result = _passwordHasher.Verify(password, user.PasswordHash);

            if (!result)
            {
                throw new NotImplementedException();
            }

            var userRoles = await _userRepository.GetUserRoles(user.Id);

            bool isAdmin = userRoles.Contains(Domain.Enums.Role.Admin);

            var token = _jwtProvider.GenerateToken(user, isAdmin);

            return token;
        }
    }
}
