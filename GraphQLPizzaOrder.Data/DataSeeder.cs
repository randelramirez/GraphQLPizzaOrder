using GraphQLPizzaOrder.Data.Entities;
using GraphQLPizzaOrder.Data.Eum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphQLPizzaOrder.Data
{
    public static class DataSeeder
    {
        public static void EnsureDataSeeding(this PizzaOrderContext dbContext
            )
        {
            if (!dbContext.OrderDetails.Any())
            {
                dbContext.OrderDetails.AddRange(new List<OrderDetail> {
                    new OrderDetail("4481  Thrash Trail", "Longview", "5033514855", 100),
                    new OrderDetail("4973  Crestview Terrace", "San Antonio", "2108543822", 180),
                    new OrderDetail("4019  Burwell Heights Road", "Sugar Land", "8329883910", 50),
                    new OrderDetail("2208  Charmaine Lane", "Lubbock", "8067739574", 120),
                });

                dbContext.SaveChanges();
            }

            if (!dbContext.PizzaDetails.Any())
            {
                dbContext.PizzaDetails.AddRange(new List<PizzaDetail>
                {
                    new PizzaDetail("Neapolitan Pizza", Toppings.ExtraCheese | Toppings.Onions, 100, 11, 1),
                    new PizzaDetail("Greek Pizza", Toppings.Mushrooms | Toppings.Pepperoni | Toppings.Bacon, 100, 11, 2),
                    new PizzaDetail("New York Style Pizza", Toppings.Sausage, 80, 11, 2),
                    new PizzaDetail("Sicilian Pizza", Toppings.NONE, 50, 9, 3),
                    new PizzaDetail("Pan Pizza", Toppings.Onions, 60, 7, 4),
                    new PizzaDetail("Thin-crust Pizza", Toppings.BlackOlives, 60, 7, 4),
                });

                dbContext.SaveChanges();
            }
        }
    }
}
