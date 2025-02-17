using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Infrastructure
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = null!;
        public int Expires { get; set; }
    }
}
