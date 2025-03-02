using EventsTask.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Interfaces
{
    public interface IUsersService
    {
        Task Register(string userName, string password);

        Task<TokenDto> Login(string userName, string password);
        Task<TokenDto> RefreshToken(TokenDto token);
    }
}
