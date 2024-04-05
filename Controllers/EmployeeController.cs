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
    public class EmployeeController : Controller
    {
        private readonly ElliedbContext _context;

        public EmployeeController(ElliedbContext context)
        {
            _context = context;
        }

        // GET: Employee
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Employees.ToListAsync());
        }

        [HttpGet("id")]
        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Employee == null)
            {
                return NotFound();
            }

            return Ok(Employee);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ActivatingTime,ImageUrl,Description")] Employee Employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Employee);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("User", Employee);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, Employee Employee)
        {
            if (id != Employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(Employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok(Employee);
        }

        // POST: Employee/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Employee = await _context.Employees.FindAsync(id);
            if (Employee != null)
            {
                _context.Employees.Remove(Employee);
            }

            await _context.SaveChangesAsync();
            return Ok(id);
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
