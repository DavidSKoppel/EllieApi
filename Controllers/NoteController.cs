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
    public class NoteController : Controller
    {
        private readonly ElliedbContext _context;

        public NoteController(ElliedbContext context)
        {
            _context = context;
        }

        // GET: Note
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Notes.ToListAsync());
        }

        [HttpGet("id")]
        // GET: Note/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Note == null)
            {
                return NotFound();
            }

            return Ok(Note);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ActivatingTime,ImageUrl,Description")] Note Note)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Note);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("User", Note);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, Note Note)
        {
            if (id != Note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(Note.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok(Note);
        }

        // POST: Note/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Note = await _context.Notes.FindAsync(id);
            if (Note != null)
            {
                _context.Notes.Remove(Note);
            }

            await _context.SaveChangesAsync();
            return Ok(id);
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
