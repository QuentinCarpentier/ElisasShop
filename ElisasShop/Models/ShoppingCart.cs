using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElisasShop.Models
{
    public class ShoppingCart
    {
        private readonly AppDbContext _appDbContext;

        // AppDbContext added to our ConfigureServices method from Startup.cs, we can use constructor injection
        public ShoppingCart(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        // GetCart static method with a IServiceProvider parameter, already used in Startup.cs
        public static ShoppingCart GetCart(IServiceProvider services)
        {
            // Access to the session 
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            // Use the services dependency injection to get access to the AppDbContext
            var context = services.GetService<AppDbContext>();

            // Get the CartId in the session. If it's empty, create a new Guid (sessionId)
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            // Set the CartId in the session
            session.SetString("CartId", cartId);

            // Return the ShoppingCart who contains the AppDbContext and the CartId
            return new ShoppingCart(context)
            {
                ShoppingCartId = cartId
            };
        }

        // Add a new Pie in our Cart
        public void AddToCart(Pie pie, int amount)
        {
            // Fetch the shoppingCartItem
            var shoppingCartItem = _appDbContext.ShoppingCartItems.SingleOrDefault(
                                        s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);

            // Check if we haven't this pie in our cart yet, create a new shoppingCartItem annd add it in the Items list
            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Pie = pie,
                    Amount = 1
                };

                // Add the new shoppingCartItem in the Items list
                _appDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            // If the pie is already existing in our cart...
            else
            {
                shoppingCartItem.Amount++;
            }

            _appDbContext.SaveChanges();
        }

        // Remove a Pie from our Cart
        public int RemoveFromCart(Pie pie)
        {
            // Fetch the shoppingCartItem
            var shoppingCartItem = _appDbContext.ShoppingCartItems.FirstOrDefault(
                                        s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);
            var localAmount = 0;

            // Check if the Cart is not null
            if (shoppingCartItem != null)
            {
                // If the Amount of the pies is greater than 1
                if (shoppingCartItem.Amount > 1)
                {
                    // Just substract the amount by 1
                    shoppingCartItem.Amount--;
                    // Set the localAmount as the current Amount of the Pies
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    // Remove the shoppingCartItem
                    _appDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            _appDbContext.SaveChanges();

            return localAmount;
        }

        // Get the Shopping Cart Items List
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            // Return the List. If the list is null, fetch the current items that are in the Cart (with CartId)
            return ShoppingCartItems ??
                (_appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                    .Include(s => s.Pie)
                    .ToList());
        }

        public void ClearCart()
        {
            // Fetch the current items that are in the Cart (with CartId)
            var cartItems = _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId);

            _appDbContext.ShoppingCartItems.RemoveRange(cartItems);
        }

        // Utilities method for the total of the Cart
        public decimal GetShoppingCartTotal()
        {
            // Fetch the current items that are in the Cart (with CartId) and count how many Pies are in there (with Amount) and do the Sum()
            var total = _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                            .Select(p => p.Pie.Price * p.Amount).Sum();

            return total;
        }
    }
}
