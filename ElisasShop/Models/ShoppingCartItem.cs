using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElisasShop.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public Pie Pie { get; set; }
        public int Amount { get; set; }
        // Store the sessionId and link a ShoppingCartItem with the ShoppingCartId
        public string ShoppingCartId { get; set; }
    }
}
