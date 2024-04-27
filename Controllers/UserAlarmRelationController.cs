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
    public class UserAlarmRelationController : GenericController
    {
        private readonly ElliedbContext _context;

        public UserAlarmRelationController(ElliedbContext context)
        {
            _context = context;
        }
        
        [HttpGet("GetAlarmsByUserId/id")]
        // GET: UserAlarmRelation/Details/5
        public async Task<IActionResult> GetAlarmsByUserId(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*var UserAlarmRelation = await _context.UserAlarmRelations
                .Where(m => m.UserId == id).Include(a => a.Alarms).ToListAsync();*/
            var UserAlarmRelation = await _context.UserAlarmRelations
                .Where(m => m.UserId == id).ToListAsync();
            List<Alarm> alarms = new List<Alarm>();
            foreach (var userAlarmRelation in UserAlarmRelation)
            {
                alarms.Add(await _context.Alarms.Where(a => a.Id == userAlarmRelation.AlarmsId).FirstOrDefaultAsync());
            }
            if (UserAlarmRelation == null)
            {
                return NotFound();
            }

            return Ok(alarms);
        }

        [HttpGet("GetPillAlarmsByUserId/id")]
        // GET: UserAlarmRelation/Details/5
        public async Task<IActionResult> GetPillAlarmsByUserId(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*var UserAlarmRelation = await _context.UserAlarmRelations
                .Where(m => m.UserId == id).Include(a => a.Alarms).ToListAsync();*/
            var UserAlarmRelation = await _context.UserAlarmRelations
                .Where(m => m.UserId == id).ToListAsync();
            List<Alarm> alarms = new List<Alarm>();
            foreach (var userAlarmRelation in UserAlarmRelation)
            {
                alarms.Add(await _context.Alarms.Where(a => a.Id == userAlarmRelation.AlarmsId).FirstOrDefaultAsync());
            }
            List<Alarm> pillAlarms = new List<Alarm>();
            foreach (var alarm in alarms)
            {
                if(alarm.Name == "Pills") 
                {
                    pillAlarms.Add(alarm);
                }
            }

            if (UserAlarmRelation == null)
            {
                return NotFound();
            }

            return Ok(pillAlarms);
        }

        // GET: UserAlarmRelation
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.UserAlarmRelations.ToListAsync());
        }

        [HttpGet("id")]
        // GET: UserAlarmRelation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var UserAlarmRelation = await _context.UserAlarmRelations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (UserAlarmRelation == null)
            {
                return NotFound();
            }

            return Ok(UserAlarmRelation);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserAlarmRelation UserAlarmRelation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(UserAlarmRelation);
                await _context.SaveChangesAsync();
            }
            return StatusCode(201, UserAlarmRelation);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Dictionary<string, object> updates)
        {
            UserAlarmRelation UserAlarmRelation = await _context.UserAlarmRelations.FindAsync(id);

            if (UserAlarmRelation == null)
            {
                return NotFound();
            }

            foreach (var update in updates)
            {
                var field = UserAlarmRelation.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(update.Key, StringComparison.OrdinalIgnoreCase));
                if (field != null && field.CanWrite)
                {
                    if (update.Value == null)
                    {
                        field.SetValue(UserAlarmRelation, null);
                    }
                    else
                    {
                        field.SetValue(UserAlarmRelation, ChangeType(update.Value.ToString(), field.PropertyType));
                    }
                }
            }

            _context.Entry(UserAlarmRelation).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return Ok(UserAlarmRelation);
        }

        // POST: UserAlarmRelation/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var UserAlarmRelation = await _context.UserAlarmRelations.FindAsync(id);
            if (UserAlarmRelation != null)
            {
                _context.UserAlarmRelations.Remove(UserAlarmRelation);
            }

            await _context.SaveChangesAsync();
            return Ok(id);
        }
    }
}
