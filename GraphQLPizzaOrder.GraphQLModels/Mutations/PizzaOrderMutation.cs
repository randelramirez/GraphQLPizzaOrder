using GraphQL;
using GraphQL.Types;
using GraphQLPizzaOrder.Core.Models;
using GraphQLPizzaOrder.Core.Services;
using GraphQLPizzaOrder.Data.Entities;
using GraphQLPizzaOrder.Data.Enum;
using GraphQLPizzaOrder.GraphQLModels.Enums;
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

            FieldAsync<OrderDetailType>(name: "createOrder", 
                arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<OrderDetailInputType>>(){ Name = "orderDetail" }
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

            FieldAsync<OrderDetailType>(name: "updateStatus",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" },
                new QueryArgument<NonNullGraphType<OrderStatusEnumType>> { Name = "status" }),
                resolve: async context =>
                {
                    var orderId = context.GetArgument<int>("id");
                    var orderStatus = context.GetArgument<OrderStatus>("status");
                    return await orderDetailService.UpdateStatusAsync(orderId, orderStatus);
                });


            FieldAsync<OrderDetailType>(name: "deletePizzaDetail",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    var pizzaDetailId = context.GetArgument<int>("id");
                    var orderDetailId = await pizzaDetailService.DeletePizzaDetailsAsync(pizzaDetailId);
                    return await orderDetailService.GetOrderDetailAsync(orderDetailId);
                });
        }
    }
}
