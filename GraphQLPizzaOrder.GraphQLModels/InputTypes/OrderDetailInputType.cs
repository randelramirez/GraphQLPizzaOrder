using GraphQL.Types;
using GraphQLPizzaOrder.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.InputTypes
{
    public class OrderDetailInputType : InputObjectGraphType<OrderDetailModel>
    {
        public OrderDetailInputType()
        {
            Name = nameof(OrderDetailInputType);

            Field(o => o.AddressLine1);
            Field(o => o.AddressLine2, nullable: true);
            Field(o => o.MobileNo);
            Field(o => o.Amount);

            Field<ListGraphType<PizzaDetailInputType>>("pizzaDetails");
        }
    }
}
