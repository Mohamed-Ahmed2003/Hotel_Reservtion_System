using Hotel_Reservtion_System.DatabaseContext;
using Hotel_Reservtion_System.DTO;
using Hotel_Reservtion_System.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Reservtion_System.Controllers
{
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly HoteldbContext _context;
        public RoomController(HoteldbContext context)
        {
            _context = context;
        }

        [Route("api/creatRoom")]
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> creatRoom(RoomDTO room)
        {
            if (ModelState.IsValid)
            {
                Room newRoom = room.convertToRoom();
                newRoom.branch = await _context.branches.FirstOrDefaultAsync(b => b.id == room.branchID);
                if (newRoom.branch == null)
                {
                    return BadRequest("branch not found");
                }
                await _context.rooms.AddAsync(newRoom);
                await _context.SaveChangesAsync();
                return Ok(newRoom);
            }
            else
            {
                String errors = string.Join("; ", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
                return BadRequest(errors);
            }
        }

        [Route("api/getAllRooms/{branchID}")]
        [HttpGet]
        [Authorize(Roles = "admin,employee")]
        public async Task<IActionResult> getAllRooms(Guid branchID)
        {
            IEnumerable<Room> rooms = await _context.rooms.Where(r => r.branch != null && r.branch.id == branchID).Include(r =>  r.branch)
                .ToListAsync();
            return Ok(rooms);
        }

        [Route("api/getAvailableRooms/{branchID}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> getAvailableRooms(Guid branchID)
        {
            IEnumerable<Room> rooms = await _context.rooms.Where(r => r.branch != null && r.branch.id == branchID && r.availability == true).Include(r => r.branch)
                .ToListAsync();
           
            return Ok(rooms);
        }

        [Route("api/updateRoom/{roomID}")]
        [HttpPut]
        [Authorize(Roles = "admin,employee")]
        public async Task<IActionResult> updateRoom(Guid roomID, RoomDTO room)
        {
            if (ModelState.IsValid)
            {
                Room? existingRoom = await _context.rooms.Include(r=>r.branch).FirstOrDefaultAsync(r => r.id == roomID);
                if (existingRoom == null)
                {
                    return NotFound("Room not found");
                }
                existingRoom.roomType = room.roomType;
                existingRoom.roomCode = room.roomCode;
                existingRoom.price = room.price;
                existingRoom.availability = room.availability;
                await _context.SaveChangesAsync();
                return Ok(existingRoom);
            }
            else
            {
                String errors = string.Join("; ", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
                return BadRequest(errors);
            }
        }

        [Route("api/deleteRoom/{roomID}")]
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> deleteRoom(Guid roomID)
        {
            Room? existingRoom = await _context.rooms.FirstOrDefaultAsync(r => r.id == roomID);
            if (existingRoom == null)
            {
                return NotFound("Room not found");
            }
            _context.rooms.Remove(existingRoom);
            await _context.SaveChangesAsync();
            return Ok("Room deleted successfully");
        }
    }
}
