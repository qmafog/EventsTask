using EventsTask.Application.Common.Dtos;
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
        Task<TokenDto?> RefreshToken(TokenDto token);
        TokenDto GenerateToken(Guid userId, bool isAdmin, bool populateExp);
    }
}
