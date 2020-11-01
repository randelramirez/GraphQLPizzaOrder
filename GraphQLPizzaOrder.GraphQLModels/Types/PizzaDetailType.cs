using GraphQL.Types;
using GraphQLPizzaOrder.Data.Entities;
using GraphQLPizzaOrder.Data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.GraphQLModels.Types
{
    public class PizzaDetailType : ObjectGraphType<PizzaDetail>
    {
        public PizzaDetailType()
        {
            Name = nameof(PizzaDetailType);
            Field(p => p.Id);
            Field(p => p.Name);
            Field(p => p.OrderDetailId);
            Field(p => p.Price);

            Field<StringGraphType>(name: nameof(Toppings), 
                resolve: context => context.Source.Toppings.ToString());
        }
    }
}
