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
    public class AlarmController : Controller
    {
        private readonly ElliedbContext _context;

        public AlarmController(ElliedbContext context)
        {
            _context = context;
        }

        // GET: Alarm
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Alarms.ToListAsync());
        }

        [HttpGet("id")]
        // GET: Alarm/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Alarm = await _context.Alarms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Alarm == null)
            {
                return NotFound();
            }

            return Ok(Alarm);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ActivatingTime,ImageUrl,Description")] Alarm Alarm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Alarm);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("User", Alarm);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, Alarm Alarm)
        {
            if (id != Alarm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Alarm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlarmExists(Alarm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok(Alarm);
        }

        // POST: Alarm/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Alarm = await _context.Alarms.FindAsync(id);
            if (Alarm != null)
            {
                _context.Alarms.Remove(Alarm);
            }

            await _context.SaveChangesAsync();
            return Ok(id);
        }

        private bool AlarmExists(int id)
        {
            return _context.Alarms.Any(e => e.Id == id);
        }
    }
}
