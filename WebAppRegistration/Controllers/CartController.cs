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

        public async Task<IActionResult> Index()
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Unauthorized();
            }

            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Unauthorized();
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null || product.StockQuantity <= 0)
            {
                return StatusCode(500, "Товару немає в наявності.");
            }

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

            if (cartItem != null)
            {
                if (cartItem.Quantity + 1 > product.StockQuantity)
                {
                    return StatusCode(500, "Недостатньо товару на складі.");
                }
                cartItem.Quantity++;
            }
            else
            {
                _context.CartItems.Add(new CartItem { UserId = userId, ProductId = productId, Quantity = 1 });
            }

            await _context.SaveChangesAsync();
            return Ok("Товар успішно додано");
        }

        [HttpPost]
        public async Task<IActionResult> ModifyCartItem(int productId, int increment)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Unauthorized();
            }

            try
            {
                var cartItem = await _context.CartItems
                    .Include(ci => ci.Product)
                    .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

                if (cartItem == null)
                {
                    return NotFound("Позицію не знайдено.");
                }

                int newQuantity = cartItem.Quantity + increment;

                if (newQuantity < 0)
                {
                    return BadRequest("Кількість не може бути від'ємною.");
                }

                if (newQuantity == 0)
                {
                    _context.CartItems.Remove(cartItem);
                }
                else
                {
                    if (newQuantity > cartItem.Product.StockQuantity)
                    {
                        return BadRequest("Недостатньо товару на складі.");
                    }
                    cartItem.Quantity = newQuantity;
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Сталась помилка, повторіть дію пізніше");
            }
        }
    }
}