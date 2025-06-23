using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventVenueBookingSystem.Data;
using EventVenueBookingSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventVenueBookingSystem.Controllers
{
    public class EventController : Controller
    {
        private readonly EventVenueBookingSystemDbContext _context;

        public EventController(EventVenueBookingSystemDbContext context)
        {
            _context = context;
        }

        // GET: /Event
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Include(e => e.EventType)
                .ToListAsync();
            return View(events);
        }

        // GET: /Event/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            var eventTypes = _context.EventTypes.ToList();
            ViewBag.EventTypeId = new SelectList(eventTypes, "Id", "Name");

            return View();
        }
        // POST: /Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            if (ModelState.IsValid)
            {
                // Generate a new GUID-based string ID
                @event.Id = Guid.NewGuid().ToString();

                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "Id", "Name", @event.EventTypeId);
            return View(@event);
        }

        // GET: /Event/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "Id", "Name", @event.EventTypeId);
            return View(@event);
        }

        // POST: /Event/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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

            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "Id", "Name", @event.EventTypeId);
            return View(@event);
        }

        // GET: /Event/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: /Event/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(string id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}