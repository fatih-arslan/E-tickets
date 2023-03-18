using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Cart
{
    public class ShoppingCart
    {        
        public AppDbContext _context { get; set; }

        public string ShoppingCartId { get; set; }


        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCart(AppDbContext context)
        {
            _context = context;
        }        

        public void AddItemToCart(Movie movie, string userId)
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.Movie.Id == movie.Id && n.UserId == userId);
            if (shoppingCartItem == null) 
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    UserId = userId,
                    Movie = movie,
                    Amount = 1
                };
                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _context.SaveChanges();
        }

        public void RemoveItemFromCart(Movie movie, string userId)
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.Movie.Id == movie.Id && n.UserId == userId);
            if (shoppingCartItem != null)
            {
                if(shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }            
            _context.SaveChanges();
        }

        public List<ShoppingCartItem> GetShoppingCartItems(string userId)
        {
            if(ShoppingCartItems == null)
            {
                ShoppingCartItems = _context.ShoppingCartItems.Where(n => n.UserId == userId).Include(n => n.Movie).ToList();
            }
            return ShoppingCartItems;
        }

        public double GetShoppingCartTotal(string userId)
        {
            var total = _context.ShoppingCartItems.Where(n => n.UserId == userId).Select(n => n.Movie.Price * n.Amount).Sum();
            return total;
        }

        public async Task ClearShoppingCartAsync(string userId)
        {
            var items = await _context.ShoppingCartItems.Where(n => n.UserId == userId).ToListAsync();
            _context.ShoppingCartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
