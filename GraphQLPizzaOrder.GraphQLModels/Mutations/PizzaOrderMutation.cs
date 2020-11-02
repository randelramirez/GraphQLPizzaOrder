using GraphQL;
using GraphQL.Types;
using GraphQLPizzaOrder.Core.Models;
using GraphQLPizzaOrder.Core.Services;
using GraphQLPizzaOrder.Data.Entities;
using GraphQLPizzaOrder.GraphQLModels.InputTypes;
using GraphQLPizzaOrder.GraphQLModels.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.Mutations
{
    public class PizzaOrderMutation : ObjectGraphType
    {
        private readonly IPizzaDetailService pizzaDetailService;
        private readonly IOrderDetailService orderDetailService;

        public PizzaOrderMutation(IPizzaDetailService pizzaDetailService, IOrderDetailService orderDetailService)
        {
            this.pizzaDetailService = pizzaDetailService;
            this.orderDetailService = orderDetailService;

            Name = nameof(PizzaOrderMutation);

            FieldAsync<OrderDetailType>(name: "createOrder", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<OrderDetailInputType>>()
                { Name = "orderDetail" }
            ),
            resolve: async context =>
            {
                // Get parameter from the GraphQL request
                var order = context.GetArgument<OrderDetailModel>("orderDetail");
                var orderDetail = new OrderDetail(order.AddressLine1, order.AddressLine2, order.MobileNo, order.Amount);
                orderDetail = await orderDetailService.CreateAsync(orderDetail);

                var pizzaDetails = order.PizzaDetails.Select(p => new PizzaDetail(p.Name, p.Toppings, p.Price, p.Size, orderDetail.Id));
                pizzaDetails = await pizzaDetailService.CreateBulkAsync(pizzaDetails, orderDetail.Id);
                orderDetail.PizzaDetails.ToList();
                return orderDetail;

            });
        }
    }
}
