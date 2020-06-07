using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAPI.Services.Exceptions
{
    public class DbValidationException : ApplicationException
    {
        public DbValidationException(string message) : base(message) { }
    }
}
