using Blomsterbinderiet.EFDbContext;
using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.Service
{
    public class OrderService
    {
        public List<Models.Order> Orders { get; set; }

        private DbGenericService<Models.Order> DbService { get; set; }
        public OrderService(DbGenericService<Models.Order> dbService)
        {
            DbService = dbService;
            ;
            Orders = dbService.GetObjectsAsync().Result.ToList();
        }

        public List<Models.Order> GetAllOrders()

        {
            Orders = DbService.GetObjectsAsync().Result.ToList();
            List<Models.Order> orders = Orders;
            return orders;
        }

        public Models.Order GetOrderById(int id)
        {
           Models.Order order = DbService.GetObjectByIdAsync(id).Result;
            return order;
        }
        public async Task UpdateOrderAsync(Models.Order order)
        {
            await DbService.UpdateObjectAsync(order);
           
        }

        public double GetOrderSum(List<OrderLine> orderLines)
        {
            double orderSum = 0;
            foreach (OrderLine orderLine in orderLines)
            {
                orderSum += orderLine.Product.Price * orderLine.Amount;
            }
            return orderSum;
        }
    }
}
