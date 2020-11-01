using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.Data.Eum
{
    public enum OrderStatus
    {
        Created = 1,
        InKitchen = 2,
        OnTheWay = 3,
        Delivered = 4,
        Canceled = 5,
    }
}
