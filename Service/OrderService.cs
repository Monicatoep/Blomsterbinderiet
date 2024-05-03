using Blomsterbinderiet.DAO;
using Blomsterbinderiet.EFDbContext;
using Blomsterbinderiet.Enums;
using Blomsterbinderiet.Migrations;
using Blomsterbinderiet.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blomsterbinderiet.Service
{
    public class OrderService : ServiceGeneric<Models.Order>
    {
        public List<Models.Order> Orders { get; set; }
        public List<Models.Delivery> Deliveries { get; set; }
        public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

        public DbGenericService<OrderLine> OrderlineService { get; set; }
        public DbGenericService<Models.Delivery> DeliveryDbService { get; set; }
        public UserService UserService { get; set; }
    
        public OrderService(DbGenericService<Models.Order> dbService, DbGenericService<OrderLine> orderlineService, DbGenericService<Models.Delivery> deliveryDbService,UserService userService) : base(dbService)
        {           
            Orders = dbService.GetObjectsAsync().Result.ToList();
            Deliveries = deliveryDbService.GetObjectsAsync().Result.ToList();
            OrderlineService = orderlineService;
            UserService = userService;
            DeliveryDbService = deliveryDbService;
        }

        public async Task<List<Models.Order>> GetAllOrdersAsync()

        {
            Orders = DbService.GetObjectsAsync().Result.ToList();
            List<Models.Order> orders = Orders;
            return orders;
        }
        public async Task<List<Models.Order>> GetAllOrdersWeekAsync()

        {
            Orders = DbService.GetObjectsAsync().Result.ToList();
            List<Models.Order> orders = (from order in Orders
                                         where order.PickUpDate < DateTime.Now.AddDays(7) && order.PickUpDate>=DateTime.Now
                                         select order).ToList();
            return orders;
        }

        public async Task<Models.Order> GetOrderByIdAsync(int id)
        {
           Models.Order order = await DbService.GetObjectByIdAsync(id);
            return order;
        }
        public async Task UpdateOrderAsync(Models.Order order)
        {
            await DbService.UpdateObjectAsync(order);
           
        }

        public double GetOrderSum(List<OrderLine> orderLines)
        {
            double orderSum = 0;
            if (orderLines == null)
            {
                return orderSum; 
            }
            
            foreach (OrderLine orderLine in orderLines)
            {
                orderSum += orderLine.Product.Price * orderLine.Amount;
            }
            return orderSum;
        }
        public async Task AddOrderAsync(Models.Order order)
        {
            Orders.Add(order);
            await DbService.AddObjectAsync(order);
        }

        public async Task AddOrderLineAsync(OrderLine orderLine)
        {
            OrderLines.Add(orderLine);
            await OrderlineService.AddObjectAsync(orderLine);
        }

        public async Task AddDeliveryAsync(Models.Delivery delivery)
        {
            Deliveries.Add(delivery);
            await DeliveryDbService.AddObjectAsync(delivery);
        }

        public async Task DenyOrderAsync(int id)
        {
            Models.Order order = DbService.GetObjectByIdAsync(id).Result;
            order.OrderStatus = Status.Afvist;
            await DbService.UpdateObjectAsync(order);
        }

        public async Task ConfirmOrderAsync(int id)
        {
            Models.Order order = DbService.GetObjectByIdAsync(id).Result;
            order.OrderStatus = Status.Bekræftet;
            await DbService.UpdateObjectAsync(order);
        }

        public async Task OrderInProgressAsync(int id, string uId)
        {
            Models.Order order = DbService.GetObjectByIdAsync(id).Result;
            order.OrderStatus = Status.Klargøres;

            string userId = uId;
            
            if (userId != null)
            {
                order.Employee = await UserService.GetUserByIdAsync(userId);
                order.EmployeeID = order.Employee.ID;
            }

            await DbService.UpdateObjectAsync(order);
        }

        public async Task ChangeOrderStatusAsync(int id)
        {
            Models.Order order = DbService.GetObjectByIdAsync(id).Result;
            if (order.OrderStatus == Status.Klargøres)
            {
                order.OrderStatus = Status.Færdig;
            }
            else if (order.OrderStatus == Status.Færdig)
            {
                order.CompletedDate = DateTime.Now;
                order.OrderStatus = Status.Udleveret;
            }
            await DbService.UpdateObjectAsync(order);
        }

        public async Task<IEnumerable<MyOrdersDAO>> GetOrdersByUserIdAsync(int id)
        {
            int userId = id;
            var orders = from o in await GetAllOrdersAsync()
                     where o.CustomerID == userId
                     join l in OrderlineService.GetObjectsAsync().Result on o.Id equals l.OrderID into ol
                     orderby o.OrderDate descending
                     select new DAO.MyOrdersDAO{ Order = o, OrderLine = ol, Amount = ol.Sum((OrderLine o) => o.Amount) };
            return orders;
        }

        public async Task CreateNewOrderAsync(User user, DateTime pickUpDate, List<OrderLine> orderLines)
        {
            Models.Order order = new(user, DateTime.Now, pickUpDate);
            await AddOrderAsync(order);
            foreach (OrderLine line in orderLines)
            {
                await AddOrderLineAsync(new OrderLine(order, line.Product, line.Amount));
            }
        }

        public async Task CreateNewOrderWithDeliveryAsync(User user, DateTime pickUpDate, List<OrderLine> orderLines, Models.Delivery delivery)
        {
            Models.Delivery newDelivery = new Models.Delivery(delivery.DeseasedName, delivery.CeremonyStart, delivery.Address);
            await AddDeliveryAsync(newDelivery);

            Models.Order order = new(user, DateTime.Now, pickUpDate);
            order.DeliveryId = newDelivery.ID;
            await AddOrderAsync(order);

            foreach (OrderLine line in orderLines)
            {
                await AddOrderLineAsync(new OrderLine(order, line.Product, line.Amount));
            }
        }

        public  IEnumerable<Models.Order> SortByDueDate()
        {
               return from order in Orders
                      orderby order.PickUpDate
                      select order;
        }
        public IEnumerable<Models.Order> SortByDueDateDes()
        {
            return from order in Orders
                   orderby order.PickUpDate descending
                   select order;
        }
        public IEnumerable<Models.Order> FilterByDueDate(DateOnly date)
        {
            TimeOnly time = TimeOnly.MinValue;
            DateTime dateTime = date.ToDateTime(time);
            Console.WriteLine(dateTime);
            return from order in Orders
                   where order.PickUpDate.Date.Equals(dateTime)
                   select order;

        }
        public IEnumerable<Models.Order> FilterByDueDateToday()
        {
            DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly time = TimeOnly.MinValue;
            DateTime dateTime = dateNow.ToDateTime(time);
           
            Console.WriteLine(dateTime);
            return from order in Orders
                   where order.PickUpDate.Date.Equals(dateTime)
                   select order;

        }
        public IEnumerable<Models.Order> SortByEmployee()
        {
            return from order in Orders
                   orderby order.EmployeeID 
                   select order;

        }
        public IEnumerable<Models.Order> SortByEmployeeDes()
        {
            return from order in Orders
                   orderby order.EmployeeID descending
                   select order;
        }
        public IEnumerable<Models.Order> FilterByEmployee(int id)
        {
            return from order in Orders
                   where order.EmployeeID == id
                   select order;
        }
        public IEnumerable<Models.Order> FilterByEmployeeNull()
        {
            return from order in Orders
                   where order.EmployeeID == null
                   select order;
        }
        public IEnumerable<Models.Order> SortByStatus()
        {
            return from order in Orders
                   orderby order.OrderStatus
                   select order;
        }
        public IEnumerable<Models.Order> SortByStatusDes()
        {
            return from order in Orders
                   orderby order.OrderStatus descending
                   select order;
        }
        public IEnumerable<Models.Order> FilterByStatus(Status status)
        {
            return from order in Orders
                   where order.OrderStatus==status
                   select order;
        }

        
    }
}
