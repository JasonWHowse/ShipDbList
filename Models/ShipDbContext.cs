using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShipDbListApplication.Models {
    public class ShipDbContext:DbContext {
        public ShipDbContext(DbContextOptions<ShipDbContext> options) : base(options) { }

        public DbSet<Ships> ShipDbs { get; set; }
    }
}
