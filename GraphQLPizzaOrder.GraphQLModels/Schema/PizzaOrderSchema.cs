using GraphQLPizzaOrder.GraphQLModels.Mutations;
using GraphQLPizzaOrder.GraphQLModels.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.Schema
{
    public class PizzaOrderSchema : GraphQL.Types.Schema
    {
        public PizzaOrderSchema(IServiceProvider services, PizzaOrderQuery query, PizzaOrderMutation mutation) : base(services)
        {
            Query = query;
            Mutation = mutation;
        }
    }
}
