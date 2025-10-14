using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebAppRegistration.Data;
using WebAppRegistration.Models;

namespace WebAppRegistration.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Groups = await _context.Groups.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                return BadRequest(error?.ErrorMessage ?? "Невірний формат даних.");
            }

            _context.Products.Add(model);
            await _context.SaveChangesAsync();

            return Ok("Новий товар успішно додано.");
        }
    }
}