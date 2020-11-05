using GraphQL.Types;
using GraphQLPizzaOrder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.Enums
{
    public class SortingDirectionEnumType : EnumerationGraphType<SortingDirection>
    {
        public SortingDirectionEnumType()
        {
            Name = nameof(SortingDirectionEnumType);
        }
    }
}
