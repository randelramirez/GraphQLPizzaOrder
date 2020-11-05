using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.ConsoleClient.Models
{
    public class NewOrdersResponse
    {
        public List<NewOrderDetail> NewOrders { get; set; }

    }

    public class NewOrderDetail
    {
        public int Id { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public int Amount { get; set; }
    }
}



