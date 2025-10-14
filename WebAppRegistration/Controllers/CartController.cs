using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppRegistration.Data;
using WebAppRegistration.Models;

namespace WebAppRegistration.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
            {
                return Unauthorized();
            }

            try
            {
                var cartItem = await _context.CartItems
                    .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

                if (cartItem != null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    var newCartItem = new CartItem
                    {
                        UserId = userId,
                        ProductId = productId,
                        Quantity = 1
                    };
                    _context.CartItems.Add(newCartItem);
                }

                await _context.SaveChangesAsync();
                return Ok("Товар успішно додано");
            }
            catch (Exception)
            {
                return StatusCode(500, "Виникла помилка додавання, повторіть спробу пізніше");
            }
        }
    }
}