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

        //IEnumerable<PizzaDetail> GetAllPizzaDetailsForOrder(int orderId);

        Task<IEnumerable<PizzaDetail>> GetAllPizzaDetailsForOrder(int orderId);
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

        public async Task<IEnumerable<PizzaDetail>> GetAllPizzaDetailsForOrder(int orderId)
        {
            return await this.context.PizzaDetails.Where(p => p.OrderDetailId == orderId).ToListAsync();
        }

        //public IEnumerable<PizzaDetail> GetAllPizzaDetailsForOrder(int orderId)
        //{
        //    return this.context.PizzaDetails.Include(o => o.OrderDetail).Where(p => p.OrderDetailId == orderId).ToList();
        //}
    }
}
