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
    public class AlarmTypeController : GenericController
    {
        private readonly ElliedbContext _context;

        public AlarmTypeController(ElliedbContext context)
        {
            _context = context;
        }

        // GET: AlarmType
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.AlarmTypes.ToListAsync());
        }

        [HttpGet("id")]
        // GET: AlarmType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var AlarmType = await _context.AlarmTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (AlarmType == null)
            {
                return NotFound();
            }

            return Ok(AlarmType);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AlarmType AlarmType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(AlarmType);
                await _context.SaveChangesAsync();
            }
            return StatusCode(201, AlarmType);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Dictionary<string, object> updates)
        {
            AlarmType AlarmType = await _context.AlarmTypes.FindAsync(id);

            if (AlarmType == null)
            {
                return NotFound();
            }

            foreach (var update in updates)
            {
                var field = AlarmType.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(update.Key, StringComparison.OrdinalIgnoreCase));
                if (field != null && field.CanWrite)
                {
                    if (update.Value == null)
                    {
                        field.SetValue(AlarmType, null);
                    }
                    else
                    {
                        field.SetValue(AlarmType, ChangeType(update.Value.ToString(), field.PropertyType));
                    }
                }
            }

            _context.Entry(AlarmType).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return Ok(AlarmType);
        }

        // POST: AlarmType/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var AlarmType = await _context.AlarmTypes.FindAsync(id);
            if (AlarmType != null)
            {
                _context.AlarmTypes.Remove(AlarmType);
            }

            await _context.SaveChangesAsync();
            return Ok(id);
        }
    }
}
