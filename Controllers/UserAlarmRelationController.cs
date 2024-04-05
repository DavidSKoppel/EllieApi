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
    public class UserAlarmRelationController : Controller
    {
        private readonly ElliedbContext _context;

        public UserAlarmRelationController(ElliedbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create([Bind("ActivatingTime,ImageUrl,Description")] UserAlarmRelation UserAlarmRelation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(UserAlarmRelation);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("User", UserAlarmRelation);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, UserAlarmRelation UserAlarmRelation)
        {
            if (id != UserAlarmRelation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(UserAlarmRelation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAlarmRelationExists(UserAlarmRelation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
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

        private bool UserAlarmRelationExists(int id)
        {
            return _context.UserAlarmRelations.Any(e => e.Id == id);
        }
    }
}
