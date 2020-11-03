using GraphQLPizzaOrder.Data;
using GraphQLPizzaOrder.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GraphQLPizzaOrder.Core.Services
{
    public interface IPizzaDetailService
    {
        Task<PizzaDetail> GetPizzaDetailAsync(int pizzaDetailId);

        Task<IEnumerable<PizzaDetail>> GetAllPizzaDetailsForOrderAsync(int orderId);

        Task<IEnumerable<PizzaDetail>> CreateBulkAsync(IEnumerable<PizzaDetail> pizzaDetails, int orderId);

        Task<int> DeletePizzaDetailsAsync(int pizzaDetailsId);
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

        public async Task<IEnumerable<PizzaDetail>> GetAllPizzaDetailsForOrderAsync(int orderId)
        {
            return await this.context.PizzaDetails.Where(p => p.OrderDetailId == orderId).ToListAsync();
        }

        public async Task<IEnumerable<PizzaDetail>> CreateBulkAsync(IEnumerable<PizzaDetail> pizzaDetails, int orderId)
        {
            await this.context.PizzaDetails.AddRangeAsync(pizzaDetails);
            await this.context.SaveChangesAsync();
            return this.context.PizzaDetails.Where(p => p.OrderDetailId == orderId);
        }

        public async Task<int> DeletePizzaDetailsAsync(int pizzaDetailsId)
        {
            var pizzaDetails = await this.context.PizzaDetails.FindAsync(pizzaDetailsId);
            if (pizzaDetails == null) return 0;

            int orderId = pizzaDetails.OrderDetailId;
            this.context.Remove(pizzaDetails);
            await this.context.SaveChangesAsync();
            return pizzaDetails.OrderDetailId;
        }
    }
}
