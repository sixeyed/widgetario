using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Widgetario.ProductApi.Entities
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductContext() : base()
        {
        }

        public ProductContext(DbContextOptions options) : base(options)
        {
        }
    }
}
