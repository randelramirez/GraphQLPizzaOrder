using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.Core.Paging
{

    // Perhaps this should be on the GraphQL project since this model is not domain specific
    // We put it here to avoid circular reference (Core is referenced in GraphQL.Models, so we can't reference  GraphQL.Models)
    public class PageResponse<T>
    {
        public List<T> Nodes { get; set; }

        public int TotalCount { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }
    }
}
