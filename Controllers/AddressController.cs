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
    public class AddressController : GenericController
    {
        private readonly ElliedbContext _context;

        public AddressController(ElliedbContext context)
        {
            _context = context;
        }

        // GET: Address
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Addresses.ToListAsync());
        }

        [HttpGet("id")]
        // GET: Address/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Address = await _context.Addresses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Address == null)
            {
                return NotFound();
            }

            return Ok(Address);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Address Address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Address);
                await _context.SaveChangesAsync();
            }
            return StatusCode(201, Address);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Dictionary<string, object> updates)
        {
            Address Address = await _context.Addresses.FindAsync(id);

            if (Address == null)
            {
                return NotFound();
            }

            foreach (var update in updates)
            {
                var field = Address.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(update.Key, StringComparison.OrdinalIgnoreCase));
                if (field != null && field.CanWrite)
                {
                    if (update.Value == null)
                    {
                        field.SetValue(Address, null);
                    }
                    else
                    {
                        field.SetValue(Address, ChangeType(update.Value.ToString(), field.PropertyType));
                    }
                }
            }

            _context.Entry(Address).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return Ok(Address);
        }

        // POST: Address/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Address = await _context.Addresses.FindAsync(id);
            if (Address != null)
            {
                _context.Addresses.Remove(Address);
            }

            await _context.SaveChangesAsync();
            return Ok(id);
        }
    }
}
