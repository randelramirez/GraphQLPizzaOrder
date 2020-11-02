using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.Core.Models
{
    public class OrderDetailModel
    {
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string MobileNo { get; set; }

        public List<PizzaDetailModel> PizzaDetails { get; set; }

        public int Amount { get; set; }
    }
}
