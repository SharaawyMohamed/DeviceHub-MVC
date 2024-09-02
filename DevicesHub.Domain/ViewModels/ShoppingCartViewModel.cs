using DevicesHub.Domain.Models;

namespace DevicesHub.Domain.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart> CartsList { get; set; }
        public decimal TotalPrices { get; set; }
        public OrderHeader OrderHeader { get; set; }

    }
}
