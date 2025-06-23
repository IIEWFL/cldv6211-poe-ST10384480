using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventVenueBookingSystem.Data;
using EventVenueBookingSystem.Models;
using EventVenueBookingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventVenueBookingSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly EventVenueBookingSystemDbContext _context;

        public BookingController(EventVenueBookingSystemDbContext context)
        {
            _context = context;
        }

        // GET: Booking
        public async Task<IActionResult> Index(string searchString)
        {
            var bookingsQuery = _context.Bookings
                .Include(b => b.Event).ThenInclude(e => e.EventType)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookingsQuery = bookingsQuery.Where(b => b.UserName.Contains(searchString));
            }

            var bookings = await bookingsQuery.ToListAsync();

            if (!bookings.Any() && !string.IsNullOrEmpty(searchString))
            {
                ViewBag.Message = "Booking not found.";
            }

            var viewModels = bookings.Select(b => new BookingViewModel
            {
                Id = b.Id,
                UserName = b.UserName,
                EventName = b.Event?.Name,
                VenueName = b.Venue?.Name,
                EventType = b.Event?.EventType?.Name,
                BookingDate = b.BookingDate,
                TimeSlot = b.TimeSlot
            });

            return View(viewModels);
        }

        // GET: Booking/Create
        public IActionResult Create()
        {
            var viewModel = new BookingViewModel
            {
                Events = _context.Events.Include(e => e.EventType).Select(e => new SelectListItem
                {
                    Value = e.Id,
                    Text = $"{e.Name} ({e.EventType.Name})"
                }).ToList(),

                Venues = _context.Venues.Select(v => new SelectListItem
                {
                    Value = v.Id,
                    Text = v.Name
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Events = _context.Events.Include(e => e.EventType).Select(e => new SelectListItem
                {
                    Value = e.Id,
                    Text = $"{e.Name} ({e.EventType.Name})"
                });

                model.Venues = _context.Venues.Select(v => new SelectListItem
                {
                    Value = v.Id,
                    Text = v.Name
                });

                return View(model);
            }

            bool exists = await _context.Bookings.AnyAsync(b =>
                b.VenueId1 == model.VenueId1 &&
                b.BookingDate == model.BookingDate &&
                b.TimeSlot == model.TimeSlot);

            if (exists)
            {
                ModelState.AddModelError("", "A booking already exists for this venue, date, and time slot.");
                model.Events = _context.Events.Include(e => e.EventType).Select(e => new SelectListItem
                {
                    Value = e.Id,
                    Text = $"{e.Name} ({e.EventType.Name})"
                });

                model.Venues = _context.Venues.Select(v => new SelectListItem
                {
                    Value = v.Id,
                    Text = v.Name
                });

                return View(model);
            }

            var booking = new Booking
            {
                UserName = model.UserName,
                EventId1 = model.EventId1,
                VenueId1 = model.VenueId1,
                BookingDate = model.BookingDate,
                TimeSlot = model.TimeSlot
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Event).ThenInclude(e => e.EventType)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            var viewModel = new BookingViewModel
            {
                Id = booking.Id,
                UserName = booking.UserName,
                EventName = booking.Event?.Name,
                VenueName = booking.Venue?.Name,
                EventType = booking.Event?.EventType?.Name,
                BookingDate = booking.BookingDate,
                TimeSlot = booking.TimeSlot
            };

            return View(viewModel);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            var viewModel = new BookingViewModel
            {
                Id = booking.Id,
                UserName = booking.UserName,
                EventId1 = booking.EventId1,
                VenueId1 = booking.VenueId1,
                BookingDate = booking.BookingDate,
                TimeSlot = booking.TimeSlot,
                Events = _context.Events.Select(e => new SelectListItem
                {
                    Value = e.Id,
                    Text = e.Name
                }),
                Venues = _context.Venues.Select(v => new SelectListItem
                {
                    Value = v.Id,
                    Text = v.Name
                })
            };

            return View(viewModel);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookingViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            bool conflict = await _context.Bookings.AnyAsync(b =>
                b.Id != model.Id &&
                b.VenueId1 == model.VenueId1 &&
                b.BookingDate == model.BookingDate &&
                b.TimeSlot == model.TimeSlot);

            if (conflict)
            {
                ModelState.AddModelError("", "A booking already exists for this venue, date, and time slot.");
                model.Events = _context.Events.Select(e => new SelectListItem
                {
                    Value = e.Id,
                    Text = e.Name
                });

                model.Venues = _context.Venues.Select(v => new SelectListItem
                {
                    Value = v.Id,
                    Text = v.Name
                });

                return View(model);
            }

            booking.UserName = model.UserName;
            booking.EventId1 = model.EventId1;
            booking.VenueId1 = model.VenueId1;
            booking.BookingDate = model.BookingDate;
            booking.TimeSlot = model.TimeSlot;

            _context.Update(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}