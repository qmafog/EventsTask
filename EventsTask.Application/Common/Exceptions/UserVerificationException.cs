using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EventsTask.Application.Common.Exceptions
{
    public class UserVerificationException : Exception
    {
        public UserVerificationException(string message) :
            base(message)
        { }
    } 
    
}
