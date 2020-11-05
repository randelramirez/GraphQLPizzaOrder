using GraphQL;
using GraphQL.Authorization;
using GraphQL.Types;
using GraphQL.Types.Relay.DataObjects;
using GraphQLPizzaOrder.Core.Constants;
using GraphQLPizzaOrder.Core.Enums;
using GraphQLPizzaOrder.Core.Helpers;
using GraphQLPizzaOrder.Core.Paging;
using GraphQLPizzaOrder.Core.Services;
using GraphQLPizzaOrder.Core.Sorting;
using GraphQLPizzaOrder.Data.Entities;
using GraphQLPizzaOrder.GraphQLModels.InputTypes;
using GraphQLPizzaOrder.GraphQLModels.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.Queries
{
    public class PizzaOrderQuery : ObjectGraphType
    {
        private readonly IOrderDetailService service;
        private readonly IPizzaDetailService pizzaDetailService;

   
        public PizzaOrderQuery(IOrderDetailService orderDetailService, IPizzaDetailService pizzaDetailService)
        {
            this.service = orderDetailService;
            this.pizzaDetailService = pizzaDetailService;
            Name = nameof(PizzaOrderQuery);
            FieldAsync<ListGraphType<OrderDetailType>>(name: "newOrders",
                resolve: async context => await orderDetailService.GetAllNewOrdersAsync());
               

            FieldAsync<PizzaDetailType>(name: "pizzaDetails",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>>() { Name = "id" }),
               resolve: async context => await pizzaDetailService.GetPizzaDetailAsync(context.GetArgument<int>("id")));

            FieldAsync<OrderDetailType>(name: "orderDetails",
              arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>>() { Name = "id" }),
             resolve: async context => await orderDetailService.GetOrderDetailAsync(context.GetArgument<int>("id"))).AuthorizeWith(AuthPolicy.RestaurantPolicy); 

            Connection<OrderDetailType>()
                   .Name("completedOrders")
                   // .Unidirectional() exposes first and after
                   .Bidirectional()
                   .Argument<CompletedOrderOrderByInputType>("orderBy", "Pass field & direction on which you want to sort data")
                   .ResolveAsync(async context =>
                   {
                       var pageRequest = new PageRequest
                       {
                           First = context.First,
                           Last = context.Last,
                           After = context.After,
                           Before = context.Before,
                           OrderBy = context.GetArgument<SortingDetails<CompletedOrdersSortingFields>>("orderBy")
                       };

                       var pageResponse = await orderDetailService.GetCompletedOrdersAsync(pageRequest);

                       (string startCursor, string endCursor) = CursorHelper.GetFirstAndLastCursor(pageResponse.Nodes.Select(x => x.Id));

                       var edge = pageResponse.Nodes.Select(x => new Edge<OrderDetail>
                       {
                           Cursor = CursorHelper.ToCursor(x.Id),
                           Node = x
                       }).ToList();

                       var connection = new Connection<OrderDetail>()
                       {
                           Edges = edge,
                           TotalCount = pageResponse.TotalCount,
                           PageInfo = new PageInfo
                           {
                               HasNextPage = pageResponse.HasNextPage,
                               HasPreviousPage = pageResponse.HasPreviousPage,
                               StartCursor = startCursor,
                               EndCursor = endCursor
                           }
                       };

                       return connection;
                   });
        }
    }
}
