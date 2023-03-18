using eTickets.Data.Cart;
using eTickets.Data.Services;
using eTickets.Data.Static;
using eTickets.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eTickets.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IMoviesService _moviesService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrdersService _ordersService;
        public OrdersController(IMoviesService moviesService, IOrdersService ordersService)
        {
            _moviesService = moviesService;            
            _ordersService = ordersService;
            _shoppingCart = ordersService.GetShoppingCart();
        }

        public async Task<IActionResult> Index()
        {
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return View(orders);
        }
        public IActionResult ShoppingCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = _shoppingCart.GetShoppingCartItems(userId);
            _shoppingCart.ShoppingCartItems= items;
            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal(userId)
            };
            return View(response);
        }

        public async Task<IActionResult> AddItemToShoppingCart(int id)
        {
            var item = await _moviesService.GetMovieByIdAsync(id);
            if(item != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _shoppingCart.AddItemToCart(item, userId);
            }
            return RedirectToAction("Index", "Movies");
        }

        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await _moviesService.GetMovieByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item, userId);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> CompleteOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = _shoppingCart.GetShoppingCartItems(userId);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

            await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync(userId);

            return View("OrderCompleted");
        }
    }
}
