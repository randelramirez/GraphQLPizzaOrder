using GraphQL;
using GraphQL.Types;
using GraphQL.Types.Relay.DataObjects;
using GraphQLPizzaOrder.Core.Enums;
using GraphQLPizzaOrder.Core.Helpers;
using GraphQLPizzaOrder.Core.Paging;
using GraphQLPizzaOrder.Core.Services;
using GraphQLPizzaOrder.Core.Sorting;
using GraphQLPizzaOrder.Data.Entities;
using GraphQLPizzaOrder.GraphQLModels.Enums;
using GraphQLPizzaOrder.GraphQLModels.InputTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.Types
{
    public class OrderDetailType : ObjectGraphType<OrderDetail>
    {
        private readonly IPizzaDetailService pizzaDetailService;
        private readonly IOrderDetailService orderDetailService;

        public OrderDetailType(IPizzaDetailService pizzaDetailService, IOrderDetailService orderDetailService)
        {
            this.pizzaDetailService = pizzaDetailService;
            this.orderDetailService = orderDetailService;
            Name = nameof(OrderDetailType);
            Field(o => o.Id);
            Field(o => o.AddressLine1);
            Field(o => o.AddressLine2,true);
            Field(o => o.MobileNo);
            Field(o => o.Amount);
            Field(o => o.Date);

            Field<OrderStatusEnumType>(
                name: nameof(OrderDetail.OrderStatus), resolve: context => context.Source.OrderStatus);

            FieldAsync<ListGraphType<PizzaDetailType>>(
             name: "pizzaDetails",
             resolve: async context => await this.pizzaDetailService.GetAllPizzaDetailsForOrderAsync(context.Source.Id));

           
        }
    }
}
