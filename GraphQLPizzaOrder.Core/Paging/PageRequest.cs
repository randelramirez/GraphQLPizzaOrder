using GraphQLPizzaOrder.Core.Enums;
using GraphQLPizzaOrder.Core.Sorting;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.Core.Paging
{
    // Perhaps this should be on the GraphQL project since this model is not domain specific
    // We put it here to avoid circular reference (Core is referenced in GraphQL.Models, so we can't reference  GraphQL.Models)
    public class PageRequest
    {
        public int? First { get; set; }
        public int? Last { get; set; }

        public string After { get; set; }

        public string Before { get; set; }

        public SortingDetails<CompletedOrdersSortingFields> OrderBy { get; set; }
    }
}
