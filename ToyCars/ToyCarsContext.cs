using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ToyCars
{
    public class ToyCarsContext : DbContext
    {
        public ToyCarsContext() : base("DbConnection") { }

        public DbSet<ToyCar> ToyCars { get; set; }
        public DbSet<RentCarInformation> RentCarsInformation { get; set; }
    }
}
