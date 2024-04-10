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
    public class InstituteController : GenericController
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
        public async Task<IActionResult> Create(Institute Institute)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Institute);
                await _context.SaveChangesAsync();
            }
            return StatusCode(201, Institute);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Dictionary<string, object> updates)
        {
            Institute Institute = await _context.Institutes.FindAsync(id);

            if (Institute == null)
            {
                return NotFound();
            }

            foreach (var update in updates)
            {
                var field = Institute.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(update.Key, StringComparison.OrdinalIgnoreCase));
                if (field != null && field.CanWrite)
                {
                    if (update.Value == null)
                    {
                        field.SetValue(Institute, null);
                    }
                    else
                    {
                        field.SetValue(Institute, ChangeType(update.Value.ToString(), field.PropertyType));
                    }
                }
            }

            _context.Entry(Institute).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
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
    }
}
