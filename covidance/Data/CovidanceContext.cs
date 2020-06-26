using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using covidance.Data;

namespace covidance.Data
{
    public class CovidanceContext : DbContext
    {
        public CovidanceContext (DbContextOptions<CovidanceContext> options)
            : base(options)
        {
        }

        public DbSet<covidance.Data.PersonInfo> Persons { get; set; }
        public DbSet<covidance.Data.RecordInfo> Records { get; set; }
    }
}
