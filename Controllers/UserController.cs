using EllieApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EllieApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : GenericController
    {
        private readonly ElliedbContext _context;

        public UserController(ElliedbContext context)
        {
            _context = context;
        }

        // GET: User
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Users.Include(e => e.ContactPerson).ToListAsync());
        }

        [HttpGet("id")]
        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var User = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (User == null)
            {
                return NotFound();
            }

            return Ok(User);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User User)
        {
            if (ModelState.IsValid)
            {
                _context.Add(User);
                await _context.SaveChangesAsync();
            }
            return StatusCode(201, User);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Dictionary<string, object> updates)
        {
            User User = await _context.Users.FindAsync(id);

            if (User == null)
            {
                return NotFound();
            }

            foreach (var update in updates)
            {
                var field = User.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(update.Key, StringComparison.OrdinalIgnoreCase));
                if (field != null && field.CanWrite)
                {
                    if (update.Value == null)
                    {
                        field.SetValue(User, null);
                    }
                    else
                    {
                        field.SetValue(User, ChangeType(update.Value.ToString(), field.PropertyType));
                    }
                }
            }

            _context.Entry(User).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return Ok(User);
        }

        // POST: User/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var User = await _context.Users.FindAsync(id);
            if (User != null)
            {
                _context.Users.Remove(User);
            }

            await _context.SaveChangesAsync();
            return Ok(id);
        }
    }
}
