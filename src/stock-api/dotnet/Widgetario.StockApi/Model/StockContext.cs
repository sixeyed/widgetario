using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Widgetario.StockApi.Entities
{
    public class StockContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public StockContext() : base()
        {
        }

        public StockContext(DbContextOptions options) : base(options)
        {
        }
    }
}
