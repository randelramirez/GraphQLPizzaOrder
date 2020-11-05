using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using GraphQLPizzaOrder.Core.Models;
using GraphQLPizzaOrder.Core.Services;
using GraphQLPizzaOrder.Data.Enum;
using GraphQLPizzaOrder.GraphQLModels.Enums;
using GraphQLPizzaOrder.GraphQLModels.Types;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.Subscriptions
{
    public class PizzaOrderSubscription : ObjectGraphType
    {
        private readonly IEventService eventService;

        public PizzaOrderSubscription(IEventService eventService)
        {
            this.eventService = eventService;
            Name = nameof(PizzaOrderSubscription);

            AddField(new EventStreamFieldType
            {
                Name = "ordderCreated",
                Type = typeof(EventDataType),
                Resolver = new FuncFieldResolver<EventDataModel>(context => context.Source as EventDataModel),
                Subscriber = new EventStreamResolver<EventDataModel>(context =>
                {
                    return this.eventService.OnCreateObservable();
                })
            });

            AddField(new EventStreamFieldType
            {
                Name = "statusUpdate",
                Arguments = new QueryArguments(new QueryArgument<NonNullGraphType<OrderStatusEnumType>> { Name = "status" }),
                Type = typeof(EventDataType),
                Resolver = new FuncFieldResolver<EventDataModel>(context => context.Source as EventDataModel),
                Subscriber = new EventStreamResolver<EventDataModel>(context =>
                {
                    OrderStatus status = context.GetArgument<OrderStatus>("status");
                    var events = eventService.OnStatusUpdateObservable();
                    return events.Where(x => x.OrderStatus == status);
                })
            });
        }
    }
}
