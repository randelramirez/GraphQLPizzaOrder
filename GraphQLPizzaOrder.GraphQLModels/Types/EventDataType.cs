using GraphQL.Types;
using GraphQLPizzaOrder.Core.Models;
using GraphQLPizzaOrder.GraphQLModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.Types
{
    public class EventDataType : ObjectGraphType<EventDataModel>
    {
        public EventDataType()
        {
            Name = nameof(EventDataType);
            Field(e => e.OrderId);
            Field<OrderStatusEnumType>("orderStatus", resolve: context => context.Source.OrderStatus);

        }
    }
}
