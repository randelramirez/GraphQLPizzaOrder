using GraphQL.Types;
using GraphQLPizzaOrder.Core.Models;
using GraphQLPizzaOrder.GraphQLModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.InputTypes
{
    public class PizzaDetailInputType : InputObjectGraphType<PizzaDetailModel>
    {
        public PizzaDetailInputType()
        {
            Name = nameof(PizzaDetailInputType);

            Field(p => p.Name);
            Field(p => p.Price);
            Field(p => p.Size);

            Field<ToppingsEnumType>(nameof(PizzaDetailModel.Toppings));
        }
    }
}
