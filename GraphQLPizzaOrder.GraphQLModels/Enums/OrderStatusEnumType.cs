using GraphQL.Types;
using GraphQLPizzaOrder.Data.Entities;
using GraphQLPizzaOrder.Data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.Enums
{

    // Another way of defining an Enum
    // We do not specify the base type, we're not using the generic EnumerationGraphType<T>
    // We use AddValue to define enum values
    public class OrderStatusEnumType : EnumerationGraphType
    {
        public OrderStatusEnumType()
        {
            Name = nameof(OrderDetail.OrderStatus);
            AddValue(nameof(OrderStatus.Created), "Order was created", 1);
            AddValue(nameof(OrderStatus.InKitchen), "Order was created", 2);
            AddValue(nameof(OrderStatus.OnTheWay), "Order was created", 3);
            AddValue(nameof(OrderStatus.Delivered), "Order was created", 4);
            AddValue(nameof(OrderStatus.Cancelled), "Order was created", 5);
        }
    }
}
