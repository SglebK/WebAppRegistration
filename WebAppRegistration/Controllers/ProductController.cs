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
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int groupId)
        {
            var products = await _context.Products
                .Where(p => p.GroupId == groupId)
                .ToListAsync();

            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return NotFound();
            }
            ViewBag.GroupName = group.Name;

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.Include(p => p.Group).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            bool isInCart = false;
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                {
                    isInCart = await _context.CartItems.AnyAsync(ci => ci.UserId == userId && ci.ProductId == id);
                }
            }

            var sameGroupRecs = await _context.Products.Where(p => p.GroupId == product.GroupId && p.Id != product.Id).OrderBy(r => Guid.NewGuid()).Take(3).ToListAsync();
            var otherGroupRecs = await _context.Products.Where(p => p.GroupId != product.GroupId).OrderBy(r => Guid.NewGuid()).Take(3).ToListAsync();

            var viewModel = new ProductDetailViewModel
            {
                Product = product,
                RecommendedProducts = sameGroupRecs.Concat(otherGroupRecs).ToList(),
                IsInCart = isInCart
            };

            return View(viewModel);
        }
    }
}