
using GraphQLPizzaOrder.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GraphQLPizzaOrder.Data.Entities;

namespace GraphQLPizzaOrder.Core.Services
{

    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllNewOrdersAsync();
    }

    public class OrderDetailService : IOrderDetailService
    {
        private readonly PizzaOrderContext context;

        public OrderDetailService(PizzaOrderContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllNewOrdersAsync()
        {
            return await this.context.OrderDetails
                .Where(o => o.OrderStatus == Data.Enum.OrderStatus.Created)
                .ToListAsync();
        }
    }
}
