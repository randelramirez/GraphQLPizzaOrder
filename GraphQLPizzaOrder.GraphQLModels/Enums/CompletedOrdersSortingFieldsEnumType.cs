using GraphQL.Types;
using GraphQLPizzaOrder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.Enums
{
    public class CompletedOrdersSortingFieldsEnumType : EnumerationGraphType<CompletedOrdersSortingFields>
    {
        public CompletedOrdersSortingFieldsEnumType()
        {
            Name = nameof(CompletedOrdersSortingFieldsEnumType);
        }
    }
}
