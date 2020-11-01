using GraphQL.Types;
using GraphQLPizzaOrder.Data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.Enums
{
    public class ToppingsEnumType : EnumerationGraphType<Toppings>
    {
        public ToppingsEnumType()
        {
            Name = nameof(ToppingsEnumType);
        }
    }
}
