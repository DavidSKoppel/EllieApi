using EllieApi.Models;
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
    public class RoleController : GenericController
    {
        private readonly ElliedbContext _context;

        public RoleController(ElliedbContext context)
        {
            _context = context;
        }

        // GET: Role
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Roles.ToListAsync());
        }

        [HttpGet("id")]
        // GET: Role/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Role = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Role == null)
            {
                return NotFound();
            }

            return Ok(Role);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Role Role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Role);
                await _context.SaveChangesAsync();
            }
            return StatusCode(201, Role);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Dictionary<string, object> updates)
        {
            Role Role = await _context.Roles.FindAsync(id);

            if (Role == null)
            {
                return NotFound();
            }

            foreach (var update in updates)
            {
                var field = Role.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(update.Key, StringComparison.OrdinalIgnoreCase));
                if (field != null && field.CanWrite)
                {
                    if (update.Value == null)
                    {
                        field.SetValue(Role, null);
                    }
                    else
                    {
                        field.SetValue(Role, ChangeType(update.Value.ToString(), field.PropertyType));
                    }
                }
            }

            _context.Entry(Role).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return Ok(Role);
        }

        // POST: Role/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Role = await _context.Roles.FindAsync(id);
            if (Role != null)
            {
                _context.Roles.Remove(Role);
            }

            await _context.SaveChangesAsync();
            return Ok(id);
        }
    }
}
