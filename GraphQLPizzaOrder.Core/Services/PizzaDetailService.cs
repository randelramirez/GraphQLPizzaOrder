using GraphQLPizzaOrder.Data;
using GraphQLPizzaOrder.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLPizzaOrder.Core.Services
{
    public interface IPizzaDetailService
    {
        Task<PizzaDetail> GetPizzaDetailAsync(int pizzaDetailId);
    }

    public class PizzaDetailService : IPizzaDetailService
    {
        private readonly PizzaOrderContext context;

        public PizzaDetailService(PizzaOrderContext context)
        {
            this.context = context;
        }

        public async Task<PizzaDetail> GetPizzaDetailAsync(int pizzaDetailId)
        {
            return await this.context.PizzaDetails.FindAsync(pizzaDetailId);
        }
    }
}
