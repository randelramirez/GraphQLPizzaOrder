using GraphQLPizzaOrder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.Core.Sorting
{
    public class SortingDetails<T>
    {
        public T Field { get; set; }

        public SortingDirection Direction { get; set; }
    }
}
