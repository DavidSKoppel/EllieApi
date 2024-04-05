using EllieApi.Model;
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
    public class AddressController : Controller
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
        public async Task<IActionResult> Create([Bind("ActivatingTime,ImageUrl,Description")] Address Address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return CreatedAtAction("User", Address);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, Address Address)
        {
            if (id != Address.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(Address.Id))
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

        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}
