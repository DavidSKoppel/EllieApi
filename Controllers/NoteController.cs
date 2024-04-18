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
    public class NoteController : GenericController
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
        public async Task<IActionResult> Create(Note Note)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Note);
                await _context.SaveChangesAsync();
            }
            return StatusCode(201, Note);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Dictionary<string, object> updates)
        {
            Note Note = await _context.Notes.FindAsync(id);

            if (Note == null)
            {
                return NotFound();
            }

            foreach (var update in updates)
            {
                var field = Note.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(update.Key, StringComparison.OrdinalIgnoreCase));
                if (field != null && field.CanWrite)
                {
                    if (update.Value == null)
                    {
                        field.SetValue(Note, null);
                    }
                    else
                    {
                        field.SetValue(Note, ChangeType(update.Value.ToString(), field.PropertyType));
                    }
                }
            }

            _context.Entry(Note).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
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

        [HttpGet("GetNoteByUserId/id")]
        public async Task<IActionResult> GetNoteByUserId(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notesRelation = await _context.Notes
                .Where(m => m.UserId == id).ToListAsync();

            return Ok(notesRelation);
        }
    }
}
