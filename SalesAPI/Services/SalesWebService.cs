using SalesAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAPI.Services
{
    public abstract class SalesWebService
    {
        protected readonly SalesWebContext _context;

        public SalesWebService(SalesWebContext context)
        {
            _context = context;
        }
    }
}
