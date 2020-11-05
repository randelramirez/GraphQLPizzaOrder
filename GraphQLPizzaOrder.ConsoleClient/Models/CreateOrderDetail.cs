using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.ConsoleClient.Models
{
    public class CreateOrderDetail
    {
        public int Id { get; set; }

        public string OrderStatus { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public List<Pizzadetail> PizzaDetails { get; set; }
    }
}
