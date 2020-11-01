using GraphQLPizzaOrder.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.Data
{
    public class PizzaOrderContext : IdentityDbContext
    {

        public PizzaOrderContext(DbContextOptions<PizzaOrderContext> options)
            : base(options)
        {
        }

        public DbSet<PizzaDetail> PizzaDetails { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
