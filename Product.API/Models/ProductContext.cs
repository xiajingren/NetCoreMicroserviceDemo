using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
           : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //初始化种子数据
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ID = 1,
                Name = "产品1",
                Stock = 100
            },
            new Product
            {
                ID = 2,
                Name = "产品2",
                Stock = 100
            });
        }
    }
}
