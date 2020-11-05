using GraphQL.Types;
using GraphQLPizzaOrder.Core.Enums;
using GraphQLPizzaOrder.Core.Sorting;
using GraphQLPizzaOrder.GraphQLModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.InputTypes
{
    public class CompletedOrderOrderByInputType : InputObjectGraphType<SortingDetails<CompletedOrdersSortingFields>>
    {
        public CompletedOrderOrderByInputType()
        {
            Field<CompletedOrdersSortingFieldsEnumType>("field", resolve: context => context.Source.Field);
            Field<SortingDirectionEnumType>("direction", resolve: context => context.Source.Direction);
        }
    }
}
