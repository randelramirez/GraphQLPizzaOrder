
using GraphQLPizzaOrder.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GraphQLPizzaOrder.Data.Entities;
using GraphQLPizzaOrder.Data.Enum;

namespace GraphQLPizzaOrder.Core.Services
{

    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllNewOrdersAsync();

        Task<OrderDetail> GetOrderDetailAsync(int id);

        Task<OrderDetail> CreateAsync(OrderDetail orderDetail);

        Task<OrderDetail> UpdateStatusAsync(int orderId, OrderStatus orderStatus);
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

        public async Task<OrderDetail> GetOrderDetailAsync(int orderId)
        {
            return await this.context.OrderDetails.FindAsync(orderId);
        }

        public async Task<OrderDetail> CreateAsync(OrderDetail orderDetail)
        {
            var newOrderDetail = await this.context.AddAsync(orderDetail);
            await this.context.SaveChangesAsync();
            //return orderDetail;
            return newOrderDetail.Entity;

        }

        public async Task<OrderDetail> UpdateStatusAsync(int orderId, OrderStatus orderStatus)
        {
            var orderDetail = await this.context.OrderDetails.FindAsync(orderId);
            if(orderDetail != null)
            {
                orderDetail.OrderStatus = orderStatus;
                await this.context.SaveChangesAsync();
            }

            return orderDetail;

        }
    }
}
