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
    public class AlarmController : GenericController
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
            return Ok(await _context.Alarms.Where(e => e.Active == true).ToListAsync());
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
        public async Task<IActionResult> Create(Alarm alarm, int[] userIds, bool isAllUsersChecked)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alarm);
                await _context.SaveChangesAsync();
            }
            return StatusCode(201, alarm);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Dictionary<string, object> updates)
        {
            Alarm alarm = await _context.Alarms.FindAsync(id);

            if (alarm == null)
            {
                return NotFound();
            }

            foreach (var update in updates)
            {
                var field = alarm.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(update.Key, StringComparison.OrdinalIgnoreCase));
                if (field != null && field.CanWrite)
                {
                    if(update.Value == null)
                    {
                        field.SetValue(alarm, null);
                    }
                    else
                    {
                        field.SetValue(alarm, ChangeType(update.Value.ToString(), field.PropertyType));
                    }
                }
            }

            _context.Entry(alarm).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return Ok(alarm);
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
    }
}
