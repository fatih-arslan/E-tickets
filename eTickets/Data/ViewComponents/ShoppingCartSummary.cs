using eTickets.Data.Cart;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eTickets.Data.ViewComponents
{
    public class ShoppingCartSummary : ViewComponent
    {
        /*private readonly ShoppingCart _shoppingcart;
        public ShoppingCartSummary(ShoppingCart shoppingcart)
        {
            _shoppingcart = shoppingcart;
        }*/

        public IViewComponentResult Invoke()
        {
            /*var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = _shoppingcart.GetShoppingCartItems();*/
            return View(0);//items.Count
        }
    }
}
