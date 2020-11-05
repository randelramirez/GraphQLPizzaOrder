
using GraphQLPizzaOrder.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GraphQLPizzaOrder.Data.Entities;
using GraphQLPizzaOrder.Data.Enum;
using GraphQLPizzaOrder.Core.Models;
using GraphQLPizzaOrder.Core.Paging;
using GraphQLPizzaOrder.Core.Helpers;

namespace GraphQLPizzaOrder.Core.Services
{

    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllNewOrdersAsync();

        Task<OrderDetail> GetOrderDetailAsync(int id);

        Task<OrderDetail> CreateAsync(OrderDetail orderDetail);

        Task<OrderDetail> UpdateStatusAsync(int orderId, OrderStatus orderStatus);

        Task<PageResponse<OrderDetail>> GetCompletedOrdersAsync(PageRequest pageRequest);
    }

    public class OrderDetailService : IOrderDetailService
    {
        private readonly PizzaOrderContext context;
        private readonly IEventService eventService;

        public OrderDetailService(PizzaOrderContext context, IEventService eventService)
        {
            this.context = context;
            this.eventService = eventService;
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
            eventService.CreateOrderEvent(new Models.EventDataModel(newOrderDetail.Entity.Id));
            return newOrderDetail.Entity;

        }

        public async Task<OrderDetail> UpdateStatusAsync(int orderId, OrderStatus orderStatus)
        {
            var orderDetail = await this.context.OrderDetails.FindAsync(orderId);
            if(orderDetail != null)
            {
                orderDetail.OrderStatus = orderStatus;
                await this.context.SaveChangesAsync();
                eventService.StatusUpdateEvent(new Models.EventDataModel(orderDetail.Id, orderDetail.OrderStatus));
            }

            return orderDetail;

        }

        public async Task<PageResponse<OrderDetail>> GetCompletedOrdersAsync(PageRequest pageRequest)
        {
            var deliveredOrders = this.context.OrderDetails
                .Where(x => x.OrderStatus == OrderStatus.Delivered);

            #region Obtain Nodes

            var dataQuery = deliveredOrders;
            if (pageRequest.First.HasValue)
            {
                // if after is specified
                // we get the last id, and only get orders = OrderStatus.Delivered is greater than the last id
                if (!string.IsNullOrEmpty(pageRequest.After))
                {
                    int lastId = CursorHelper.FromCursor(pageRequest.After);
                    dataQuery = dataQuery.Where(x => x.Id > lastId);
                }

                dataQuery = dataQuery.Take(pageRequest.First.Value);
            }
         
            // LOGIC FOR BEFORE AND LAST NOT YET VERIFIED
            if (pageRequest.Last.HasValue)
            {
                if(!string.IsNullOrEmpty(pageRequest.Before))
                {
                    int beforeId = CursorHelper.FromCursor(pageRequest.Before);
                    dataQuery = dataQuery.Where(x => x.Id > beforeId);
                }

                // dataQuery = dataQuery.TakeLast(pageRequest.Last.Value); // TakeLast might not be translated by EF to SQL(not yet tested)

                // we reverse the order so that we start on the end 
                dataQuery = dataQuery.OrderByDescending(x => x.Id).Take(pageRequest.Last.Value);
            }

            // We only sort if we have specified OrderBy
            if (pageRequest.OrderBy?.Field == Enums.CompletedOrdersSortingFields.Address)
            {
                dataQuery = (pageRequest.OrderBy.Direction == Enums.SortingDirection.DESC)
                    ? dataQuery.OrderByDescending(x => x.AddressLine1)
                    : dataQuery.OrderBy(x => x.AddressLine1);
            }
            else if (pageRequest.OrderBy?.Field == Enums.CompletedOrdersSortingFields.Amount)
            {
                dataQuery = (pageRequest.OrderBy.Direction == Enums.SortingDirection.DESC)
                    ? dataQuery.OrderByDescending(x => x.Amount)
                    : dataQuery.OrderBy(x => x.Amount);
            }
            else
            {
                dataQuery = (pageRequest.OrderBy?.Direction == Enums.SortingDirection.DESC)
                    ? dataQuery.OrderByDescending(x => x.Id)
                    : dataQuery.OrderBy(x => x.Id);
            }

            List<OrderDetail> nodes = await dataQuery?.ToListAsync();

            #endregion

            #region Obtain Flags

            //WHY DO WE NEED node.max id? and min
            int maxId = nodes.Count > 0 ? nodes.Max(x => x.Id) : 0;
            int minId = nodes.Count > 0 ? nodes.Min(x => x.Id) : 0;
            bool hasNextPage = await deliveredOrders.AnyAsync(x => x.Id > maxId);
            bool hasPrevPage = await deliveredOrders.AnyAsync(x => x.Id < minId);
            int totalCount = await deliveredOrders.CountAsync();

            #endregion

            return new PageResponse<OrderDetail>
            {
                Nodes = nodes,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPrevPage,
                TotalCount = totalCount
            };
        }
    }
}
