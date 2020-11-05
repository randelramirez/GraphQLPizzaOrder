using GraphQLPizzaOrder.GraphQLModels.Mutations;
using GraphQLPizzaOrder.GraphQLModels.Queries;
using GraphQLPizzaOrder.GraphQLModels.Subscriptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.Schema
{
    public class PizzaOrderSchema : GraphQL.Types.Schema
    {
        public PizzaOrderSchema(IServiceProvider services, PizzaOrderQuery query, PizzaOrderMutation mutation, PizzaOrderSubscription subscription) : base(services)
        {
            Query = query;
            Mutation = mutation;
            Subscription = subscription;
        }
    }
}
