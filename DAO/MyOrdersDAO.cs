using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.DAO
{
    public class MyOrdersDAO
    {
        public Order Order;
        public IEnumerable<OrderLine> OrderLine;
        public int Amount;
    }
}
