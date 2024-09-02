using DevicesHub.Domain.Models;

namespace DevicesHub.Domain.ViewModels
{
    public class OrderViewModel
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetails> OrdersDetails { get; set; }

    }
}
