using Hotel_Reservtion_System.DatabaseContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Reservtion_System.Controllers
{
    [ApiController]
    public class InoviceController : Controller
    {
        private readonly HoteldbContext _hoteldbContext;

        public InoviceController(HoteldbContext hoteldbContext)
        {
            _hoteldbContext = hoteldbContext;
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

        

    }
}
