using GraphQLPizzaOrder.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLPizzaOrder.Core.Services
{
    public interface IPizzaDetailService
    {

    }

    public class PizzaDetailService : IPizzaDetailService
    {
        private readonly PizzaOrderContext context;

        public PizzaDetailService(PizzaOrderContext context)
        {
            this.context = context;
        }
    }
}
