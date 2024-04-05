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
    public class InstituteController : Controller
    {
        private readonly ElliedbContext _context;

        public InstituteController(ElliedbContext context)
        {
            _context = context;
        }

        // GET: Institute
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Institutes.ToListAsync());
        }

        [HttpGet("id")]
        // GET: Institute/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Institute = await _context.Institutes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Institute == null)
            {
                return NotFound();
            }

            return Ok(Institute);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ActivatingTime,ImageUrl,Description")] Institute Institute)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Institute);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("User", Institute);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, Institute Institute)
        {
            if (id != Institute.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Institute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstituteExists(Institute.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok(Institute);
        }

        // POST: Institute/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Institute = await _context.Institutes.FindAsync(id);
            if (Institute != null)
            {
                _context.Institutes.Remove(Institute);
            }

            await _context.SaveChangesAsync();
            return Ok(id);
        }

        private bool InstituteExists(int id)
        {
            return _context.Institutes.Any(e => e.Id == id);
        }
    }
}
