using Hotel_Reservtion_System.DatabaseContext;
using Hotel_Reservtion_System.DTO;
using Hotel_Reservtion_System.Services;
using Hotel_Reservtion_System.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Tnef;
using System.Security.Claims;
using System.Text.Json;

namespace Hotel_Reservtion_System.Controllers
{
    [ApiController]
    public class InoviceController : Controller
    {
        private readonly HoteldbContext _hoteldbContext;
        private readonly IPaymentServices _paymentServices;

        public InoviceController(HoteldbContext hoteldbContext,IPaymentServices paymentServices)
        {
            _hoteldbContext = hoteldbContext;
            _paymentServices = paymentServices;
        }

        [HttpGet]
        [Route("api/getAllInvoices")]
        [Authorize(Roles = "admin,employee")]
        public async Task<IActionResult> GetAllInvoices()
        {
            var invoices = await _hoteldbContext.invoices
                .Include(i => i.booking)
                .ThenInclude(b => b.user)
                .Include(i => i.booking)
                .ThenInclude(b => b.room)
                .Include(c=>c.createdBy)
                .ToListAsync();
            return Ok(invoices);
        }

        [HttpGet]
        [Route("api/getInvoiceById/{id}")]
        [Authorize(Roles = "admin,employee")]
        public async Task<IActionResult> GetInvoiceById(Guid id)
        {
            var invoice = await _hoteldbContext.invoices
                .Include(i => i.booking)
                .ThenInclude(b => b.user)
                .Include(i => i.booking)
                .ThenInclude(b => b.room)
                .FirstOrDefaultAsync(i => i.id == id);
            if (invoice == null)
            {
                return NotFound("Invoice not found");
            }
            return Ok(invoice);
        }
        [HttpPost]
        [Route("api/payInvoice")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> PayInvoice(OnlinePaymentDTO onlinePaymentDTO )
        {
            string userEmail = User.FindFirstValue(ClaimTypes.Email);
            var session = await _paymentServices.CreateCheckoutSessionAsync(onlinePaymentDTO.amount, userEmail);
            
            return Ok(new { url = session.Url ,sessionId=session.Id} );
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            bool IsPay =await _paymentServices.HandleWebHookAsync(Request);
            if(IsPay)
            {
                var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                using var json = JsonDocument.Parse(requestBody);
                string? email = json.RootElement.GetProperty("data").GetProperty("object").GetProperty("customer_details").GetProperty("email").GetString(); 
                var Invoive = await _hoteldbContext.invoices
                    .Include(i => i.booking)
                    .ThenInclude(b => b.user)
                    .Include(i => i.booking)
                    .ThenInclude(b => b.room)
                    .FirstOrDefaultAsync(i => i.booking.user.email == email && i.status=="unpaid");
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
