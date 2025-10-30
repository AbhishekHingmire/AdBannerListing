using AdBannerListings.Data;
using AdBannerListings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdBannerListings.Controllers
{
    public class BannerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BannerController> _logger;

        public BannerController(ApplicationDbContext context, ILogger<BannerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Banner
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Loading banner list");
                var banners = await _context.Banners.ToListAsync();
                _logger.LogInformation($"Found {banners.Count} banners");
                return View(banners);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading banners");
                ViewBag.ErrorMessage = $"Database error: {ex.Message}";
                return View(new List<Banner>());
            }
        }

        // GET: Banner/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // GET: Banner/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Banner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ImageURL,RedirectURL,Latitude,Longitude,Radius")] Banner banner)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    banner.Id = Guid.NewGuid();
                    _context.Add(banner);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Created new banner with ID: {banner.Id}");
                    return RedirectToAction(nameof(Index));
                }
                return View(banner);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating banner");
                ModelState.AddModelError("", $"Error creating banner: {ex.Message}");
                return View(banner);
            }
        }

        // GET: Banner/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        // POST: Banner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,ImageURL,RedirectURL,Latitude,Longitude,Radius")] Banner banner)
        {
            if (id != banner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(banner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerExists(banner.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(banner);
        }

        // GET: Banner/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // POST: Banner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner != null)
            {
                _context.Banners.Remove(banner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannerExists(Guid id)
        {
            return _context.Banners.Any(e => e.Id == id);
        }
    }
}