﻿using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.Service
{
    public class OrderService
    {
        public List<Order> Orders { get; set; }

        private DbGenericService<Order> DbService { get; set; }
        public OrderService(DbGenericService<Order> dbService)
        {
            DbService = dbService;
            ;
            Orders = dbService.GetObjectsAsync().Result.ToList();
        }

        public List<Order> GetAllOrders()

        {
            List<Order> orders = Orders;
            return orders;
        }

    }
}