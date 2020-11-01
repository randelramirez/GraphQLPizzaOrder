using GraphQLPizzaOrder.GraphQLModels.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.Schema
{
    public class PizzaOrderSchema : GraphQL.Types.Schema
    {
        public PizzaOrderSchema(IServiceProvider services, PizzaOrderQuery query) : base(services)
        {

            Query = query;
        }
    }
}
