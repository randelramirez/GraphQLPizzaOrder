using GraphQLPizzaOrder.Data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.Core.Models
{
    public class EventDataModel
    {
        public int OrderId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public EventDataModel(int orderId, OrderStatus orderStatus = OrderStatus.Created)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
        }
    }
}
