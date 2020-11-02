using GraphQL;
using GraphQL.Types;
using GraphQLPizzaOrder.Core.Services;
using GraphQLPizzaOrder.GraphQLModels.Types;
using System;
using System.Collections.Generic;
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
             resolve: async context => await orderDetailService.GetOrderDetailAsync(context.GetArgument<int>("id")));
        }
    }
}
