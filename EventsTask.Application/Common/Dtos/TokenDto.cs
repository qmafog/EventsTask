using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Common.Dtos
{
    public record TokenDto(string AccessToken, string RefreshToken);

}
