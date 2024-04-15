namespace Blomsterbinderiet.Models
{
    public class BasketItem
    {
        public int ProductID { get; set; }
        public int Amount { get; set; }

        public BasketItem()
        {
        }

        public override string ToString()
        {
            return $"{{{nameof(ProductID)}={ProductID.ToString()}, {nameof(Amount)}={Amount.ToString()}}}";
        }
    }
}
