using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Application.Common.Exceptions
{
    public class AuthorizeConfigurationException : Exception
    {
        public AuthorizeConfigurationException(string name)
            : base($"Authorization unavailable. {name} unconfigured.")
        { }
    }  
}
