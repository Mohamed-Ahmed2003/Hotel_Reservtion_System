using Hotel_Reservtion_System.DatabaseContext;
using Hotel_Reservtion_System.DTO;
using Hotel_Reservtion_System.Entity;
using Hotel_Reservtion_System.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Security.Claims;
using Invoice = Hotel_Reservtion_System.Entity.Invoice;

namespace Hotel_Reservtion_System.Controllers
{
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly HoteldbContext _context;
        private readonly IInvoiceService _invoiceService;
        private readonly IEmailServices _emailServices;
        
        public BookingController(HoteldbContext context,IInvoiceService invoiceService, IEmailServices emailServices, IPaymentServices paymentServices)
        {
            _invoiceService = invoiceService;
            _context = context;
            _emailServices = emailServices;
        }

        [HttpGet]
        [Route("api/getAllBookings")]
        [Authorize(Roles ="admin,employee")]
        public async Task<IActionResult> getAllBookings()
        {
            var bookings = await _context.bookings
                .Include(b=>b.user)
                .Include(b=>b.room)
                .ToListAsync();
            return Ok(bookings);
        }

        [HttpGet]
        [Route("api/getAllBooking/{id}")]
        [Authorize(Roles = "admin,employee")]
        public async Task<IActionResult> getAllBookingToBranch(Guid id)
        {
            var bookings = await _context.bookings
                .Include(b => b.user)
                .Include(b => b.room)
                .Where(b => b.room.branch.id == id)
                .ToListAsync();
            if (bookings == null)
            {
                return NotFound("No bookings found for the specified branch");
            }
            return Ok(bookings);
        }

        [HttpGet]
        [Route("api/getBookingById/{id}")]
        [Authorize(Roles = "admin,employee")]
        public async Task<IActionResult> getBookingById(Guid id)
        {
            var booking = await _context.bookings
                .Include(b=>b.user)
                .Include(b=>b.room)
                .FirstOrDefaultAsync(b => b.id == id);
            if (booking == null)
            {
                return NotFound("Booking not found");
            }
            return Ok(booking);
        }

        [HttpPost]
        [Route("api/manualBooking")]
        [Authorize(Roles = "employee")]
        public async Task<IActionResult> manualBooking( BookingDTO bookingDTO,[FromQuery]double taxAmount,[FromQuery]double discount)
        {
            if(!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                
                return BadRequest(errors);
            }
            if(bookingDTO.user.role == null)
            {
                return BadRequest("User information is required");
            }
            if (bookingDTO.user.role.ToLower() != "user")
            {
                return BadRequest("Role must be user for booking");
            }
            var email = User?.FindFirstValue(ClaimTypes.Email);
            User? employee = await _context.Users.FirstOrDefaultAsync(u => u.email == email.ToString());
            User? newUser = _context.Users.FirstOrDefault(u => u.email == bookingDTO.user.email);
            if(newUser == null)
            {
                newUser = bookingDTO.user;
                newUser.id = Guid.NewGuid();
                newUser.isApproved = "true";
                _context.Users.Add(newUser);
                _context.SaveChanges();
            }
            Room? room = await _context.rooms.FirstOrDefaultAsync(r => r.id == bookingDTO.roomId);
            if(room == null)
            {
                return NotFound("Room not found");
            }
            if(room.cheakout>bookingDTO.checkIn)
            {
                return BadRequest("Room is already booked");
            }
            Booking newBooking = new Booking
            {
                id = Guid.NewGuid(),
                user = newUser,
                room = room,
                checkIn = bookingDTO.checkIn,
                checkOut = bookingDTO.checkOut,
                status = bookingDTO.status,
               
            };
            room.cheakout = bookingDTO.checkOut;
            Invoice newInvoice = _invoiceService.MakeInvoice(newBooking, taxAmount, discount, employee);
            await _emailServices.sendEmail(newUser.email, newInvoice);
            await _context.invoices.AddAsync(newInvoice);
            await _context.bookings.AddAsync(newBooking);
            await _context.SaveChangesAsync();
            return Ok(new invoiceDTO { booking=newBooking,amount=newInvoice.finalAmount, discount=newInvoice.discount , createdBy=newInvoice.createdBy.name});
        }

        [HttpPut]
        [Route("api/updateBooking/{id}")]
        [Authorize(Roles = "employee")]
        public async Task <IActionResult> updateBooking(Guid id, BookingDTO bookingDTO)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(errors);
            }
            Booking? existingBooking = await _context.bookings
                .Include(b => b.room)
                .Include(b => b.user)
                .FirstOrDefaultAsync(b => b.id == id);
            if (existingBooking == null)
            {
                return NotFound("Booking not found");
            }
            Room? room = await _context.rooms.FirstOrDefaultAsync(r => r.id == bookingDTO.roomId);
            if(room == null)
            {
                return NotFound("Room not found");
            }
            if(room.cheakout > bookingDTO.checkIn)
            {
                return BadRequest("Room is already booked");
            }
            User existUser = await _context.Users.FirstOrDefaultAsync(u => u.email == bookingDTO.user.email);
            if(existUser == null)
            {
                existUser = bookingDTO.user;
                existUser.id = Guid.NewGuid();
                _context.Users.Add(existUser);
                await _context.SaveChangesAsync();
            }
            existingBooking.room = room;
            existingBooking.room.cheakout = bookingDTO.checkOut;
            existingBooking.checkIn = bookingDTO.checkIn;
            existingBooking.checkOut = bookingDTO.checkOut;
            existingBooking.status = bookingDTO.status;
            existingBooking.user =existUser;
            await _context.SaveChangesAsync();
            return Ok(existingBooking);
        }


        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("api/onlineBooking")]
        public async Task<IActionResult> onlineBooking(BookingDTO bookingDTO)
        {
            if(!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(errors);
            }
            if (bookingDTO.user.role == null)
            {
                return BadRequest("User information is required");
            }
            User? newUser = _context.Users.FirstOrDefault(u => u.email == bookingDTO.user.email);
            if (newUser == null)
            {
                newUser = bookingDTO.user;
                newUser.id = Guid.NewGuid();
                newUser.isApproved = "true";
                _context.Users.Add(newUser);
                _context.SaveChanges();
            }
            Room? room = await _context.rooms.FirstOrDefaultAsync(r => r.id == bookingDTO.roomId);
            if (room == null)
            {
                return NotFound("Room not found");
            }
            if (room.cheakout > bookingDTO.checkIn)
            {
                return BadRequest("Room is already booked");
            }
            Booking newBooking = new Booking
            {
                id = Guid.NewGuid(),
                user = newUser,
                room = room,
                checkIn = bookingDTO.checkIn,
                checkOut = bookingDTO.checkOut,
                status = "pending",

            };
            room.cheakout = bookingDTO.checkOut;

            Invoice newInvoice = _invoiceService.MakeInvoice(newBooking, 0.1, 0, null);
            newInvoice.status = "unpaid";
            await _context.invoices.AddAsync(newInvoice);
            await _context.bookings.AddAsync(newBooking);
            await _context.SaveChangesAsync();
            return Ok(newInvoice);
        }
    }
}
