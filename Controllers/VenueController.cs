using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventVenueBookingSystem.Data;
using EventVenueBookingSystem.Models;
using EventVenueBookingSystem.Services;

namespace EventVenueBookingSystem.Controllers
{
    public class VenueController : Controller
    {
        private readonly EventVenueBookingSystemDbContext _context;
        private readonly AzureBlobStorageService _blob;

        public VenueController(EventVenueBookingSystemDbContext context, AzureBlobStorageService blob)
        {
            _context = context;
            _blob = blob;
        }

        public async Task<IActionResult> Index()
        {
            var venues = await _context.Venues.ToListAsync();
            return View(venues);
        }

        public async Task<IActionResult> Details(string id)
        {
            var v = await _context.Venues.FindAsync(id);
            if (v == null) { TempData["Error"] = "Venue not found."; return RedirectToAction(nameof(Index)); }
            return View(v);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Venue venue, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                venue.Id = Guid.NewGuid().ToString(); // Manually assign string ID

                if (ImageFile != null)
                    venue.ImageUrl = await _blob.UploadFileAsync(ImageFile);

                _context.Venues.Add(venue);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Venue created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var v = await _context.Venues.FindAsync(id);
            if (v == null) { TempData["Error"] = "Venue not found."; return RedirectToAction(nameof(Index)); }
            return View(v);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Venue venue, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                    venue.ImageUrl = await _blob.UploadFileAsync(ImageFile);

                _context.Venues.Update(venue);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Venue updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var v = await _context.Venues.Include(ven => ven.Bookings).FirstOrDefaultAsync(ven => ven.Id == id);
            if (v == null) { TempData["Error"] = "Venue not found."; return RedirectToAction(nameof(Index)); }

            if (v.Bookings != null && v.Bookings.Any())
            {
                TempData["Error"] = "Cannot delete venue with existing bookings.";
                return RedirectToAction(nameof(Index));
            }

            _context.Venues.Remove(v);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Venue deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}