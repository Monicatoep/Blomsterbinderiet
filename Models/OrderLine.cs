﻿namespace Blomsterbinderiet.Models
{
    public class OrderLine
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }

        public OrderLine()
        {
        }

        public override string ToString()
        {
            return $"{{{nameof(ID)}={ID.ToString()}, {nameof(OrderID)}={OrderID.ToString()}, {nameof(Order)}={Order}, {nameof(ProductID)}={ProductID.ToString()}, {nameof(Product)}={Product}, {nameof(Amount)}={Amount.ToString()}}}";
        }
    }
}
