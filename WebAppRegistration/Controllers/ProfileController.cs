using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userLogin = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin);

            if (user == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProfileUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                return BadRequest(error?.ErrorMessage ?? "Невірний формат даних.");
            }

            var userLogin = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin);

            if (user == null)
            {
                return NotFound("Користувача не знайдено.");
            }

            bool emailChanged = user.Email != model.Email;
            if (emailChanged && await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                return BadRequest("Користувач з таким email вже існує.");
            }

            user.Name = model.Name;
            user.Email = model.Email;

            await _context.SaveChangesAsync();

            var newClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim("FullName", user.Name),
                new Claim(ClaimTypes.DateOfBirth, user.Birthday.ToString("o"))
            };

            var claimsIdentity = new ClaimsIdentity(newClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var userLogin = User.Identity.Name;

            var user = await _context.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Login == userLogin);

            if (user == null)
            {
                return NotFound();
            }

            user.DateDeleted = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.Include(p => p.Group).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var sameGroupRecs = await _context.Products
                .Where(p => p.GroupId == product.GroupId && p.Id != product.Id)
                .OrderBy(r => Guid.NewGuid())
                .Take(3)
                .ToListAsync();

            var otherGroupRecs = await _context.Products
                .Where(p => p.GroupId != product.GroupId)
                .OrderBy(r => Guid.NewGuid())
                .Take(3)
                .ToListAsync();

            var viewModel = new ProductDetailViewModel
            {
                Product = product,
                RecommendedProducts = sameGroupRecs.Concat(otherGroupRecs).ToList()
            };

            return View(viewModel);
        }
    }
}