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
    public class UserController : Controller
    {
        private readonly ElliedbContext _context;

        public UserController(ElliedbContext context)
        {
            _context = context;
        }

        // GET: User
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpGet("id")]
        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var User = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (User == null)
            {
                return NotFound();
            }

            return Ok(User);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ActivatingTime,ImageUrl,Description")] User User)
        {
            if (ModelState.IsValid)
            {
                _context.Add(User);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("User", User);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, User User)
        {
            if (id != User.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(User);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(User.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok(User);
        }

        // POST: User/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var User = await _context.Users.FindAsync(id);
            if (User != null)
            {
                _context.Users.Remove(User);
            }

            await _context.SaveChangesAsync();
            return Ok(id);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
