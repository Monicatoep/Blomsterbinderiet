using System.ComponentModel;

namespace Blomsterbinderiet.Models
{
    public class BasketItem
    {
        public int ProductID { get; set; }

        [DisplayName("Mængde")]
        public int Amount { get; set; }

        public BasketItem()
        {
        }

        public override string ToString()
        {
            return $"{{{nameof(ProductID)}={ProductID}, {nameof(Amount)}={Amount}}}";
        }
    }
}
