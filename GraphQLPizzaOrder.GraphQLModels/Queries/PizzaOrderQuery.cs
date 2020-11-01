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

        public PizzaOrderQuery(IOrderDetailService service)
        {
            this.service = service;

            Name = nameof(PizzaOrderQuery);
            FieldAsync<ListGraphType<OrderDetailType>>(name: "newOrders", 
                resolve: async context => await service.GetAllNewOrdersAsync());
        }
    }
}
