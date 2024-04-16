using EllieApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllieApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : GenericController
    {
        private readonly ElliedbContext _context;

        public RoomController(ElliedbContext context)
        {
            _context = context;
        }

        // GET: Room
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Rooms.ToListAsync());
        }

        [HttpGet("id")]
        // GET: Room/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Room == null)
            {
                return NotFound();
            }

            return Ok(Room);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Room Room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Room);
                await _context.SaveChangesAsync();
            }
            return StatusCode(201, Room);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Dictionary<string, object> updates)
        {
            Room Room = await _context.Rooms.FindAsync(id);

            if (Room == null)
            {
                return NotFound();
            }

            foreach (var update in updates)
            {
                var field = Room.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(update.Key, StringComparison.OrdinalIgnoreCase));
                if (field != null && field.CanWrite)
                {
                    if (update.Value == null)
                    {
                        field.SetValue(Room, null);
                    }
                    else
                    {
                        field.SetValue(Room, ChangeType(update.Value.ToString(), field.PropertyType));
                    }
                }
            }

            _context.Entry(Room).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return Ok(Room);
        }

        // POST: Room/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Room = await _context.Rooms.FindAsync(id);
            if (Room != null)
            {
                _context.Rooms.Remove(Room);
            }

            await _context.SaveChangesAsync();
            return Ok(id);
        }
    }
}
