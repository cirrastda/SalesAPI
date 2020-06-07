using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAPI.Services.Exceptions
{
    public class DuplicateException<T>: DbValidationException where T : class
    {
        public DuplicateException(DbSet<T> entity) : base(typeof(T).Name
            + " already exists") { }
    }
}
