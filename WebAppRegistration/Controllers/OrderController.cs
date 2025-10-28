using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppRegistration.Data;
using WebAppRegistration.Models;

namespace WebAppRegistration.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Ваш кошик порожній.";
                return RedirectToAction("Index", "Cart");
            }

            foreach (var item in cartItems)
            {
                if (item.Quantity > item.Product.StockQuantity)
                {
                    TempData["ErrorMessage"] = $"Недостатньо товару '{item.Product.Name}' на складі. Доступно: {item.Product.StockQuantity} шт.";
                    return RedirectToAction("Index", "Cart");
                }
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = cartItems.Sum(ci => ci.Quantity * ci.Product.Price)
            };

            foreach (var item in cartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    PriceAtOrder = item.Product.Price
                });

                item.Product.StockQuantity -= item.Quantity;
            }

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Замовлення №{order.Id} успішно створено!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RepeatOrder(int orderId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var orderToRepeat = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (orderToRepeat == null)
            {
                return NotFound("Замовлення не знайдено або воно не належить вам.");
            }

            var currentUserCart = _context.CartItems.Where(ci => ci.UserId == userId);
            _context.CartItems.RemoveRange(currentUserCart);

            var unavailableItems = new List<string>();

            foreach (var item in orderToRepeat.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);

                if (product == null)
                {
                    unavailableItems.Add($"Товар з ID {item.ProductId} більше не існує.");
                    continue;
                }

                if (product.StockQuantity < item.Quantity)
                {
                    unavailableItems.Add($"Для товару '{product.Name}' недостатньо залишків (потрібно {item.Quantity}, доступно {product.StockQuantity}).");
                    continue;
                }

                _context.CartItems.Add(new CartItem
                {
                    UserId = userId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            await _context.SaveChangesAsync();

            if (unavailableItems.Any())
            {
                TempData["WarningMessage"] = "Замовлення повторено частково. Деякі товари не були додані до кошику: " + string.Join("; ", unavailableItems);
            }
            else
            {
                TempData["SuccessMessage"] = "Замовлення успішно додано до кошику.";
            }

            return RedirectToAction("Index", "Cart");
        }
    }
}