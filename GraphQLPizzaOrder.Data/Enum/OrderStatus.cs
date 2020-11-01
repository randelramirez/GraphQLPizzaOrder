using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.Data.Enum
{
    public enum OrderStatus
    {
        Created = 1,
        InKitchen = 2,
        OnTheWay = 3,
        Delivered = 4,
        Cancelled = 5,
    }
}
