using GraphQL.Types;
using GraphQLPizzaOrder.Core.Services;
using GraphQLPizzaOrder.Data.Entities;
using GraphQLPizzaOrder.GraphQLModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.Types
{
    public class OrderDetailType : ObjectGraphType<OrderDetail>
    {
        private readonly IPizzaDetailService pizzaDetailService;

        public OrderDetailType(IPizzaDetailService pizzaDetailService)
        {
            this.pizzaDetailService = pizzaDetailService;

            Name = nameof(OrderDetailType);
            Field(o => o.Id);
            Field(o => o.AddressLine1);
            Field(o => o.AddressLine2);
            Field(o => o.MobileNo);
            Field(o => o.Amount);
            Field(o => o.Date);

            Field<OrderStatusEnumType>(
                name: nameof(OrderDetail.OrderStatus), resolve: context => context.Source.OrderStatus);

            FieldAsync<ListGraphType<PizzaDetailType>>(
             name: "pizzaDetails",
             resolve: async context => await this.pizzaDetailService.GetAllPizzaDetailsForOrder(context.Source.Id));
        
        }
    }
}
