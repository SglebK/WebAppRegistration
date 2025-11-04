using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebAppRegistration.Data;

namespace WebAppRegistration.Controllers.Api
{
    [ApiController]
    public class GroupApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GroupApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("/api/group/{slug}")]
        public async Task<IActionResult> GetGroupBySlug(string slug)
        {
            var group = await _context.Groups
                .Include(g => g.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Slug == slug);

            if (group == null)
            {
                return NotFound();
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            if (!string.IsNullOrEmpty(group.ImageUrl))
            {
                group.ImageUrl = $"{baseUrl}/images/{group.ImageUrl}";
            }

            foreach (var product in group.Products)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    product.ImageUrl = $"{baseUrl}/images/{product.ImageUrl}";
                }
            }

            return Ok(group);
        }
    }
}