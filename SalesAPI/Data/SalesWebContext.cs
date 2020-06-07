using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Models.Entities;

namespace SalesAPI.Data
{
    public class SalesWebContext: DbContext
    {
        public DbSet<Department> Department { get; set; }
        public DbSet<Seller> Seller { get; set; }
        public DbSet<SalesRecord> SalesRecord { get; set; }

        public SalesWebContext(DbContextOptions<SalesWebContext> options)
            : base(options)
        {
        }


    }
}
