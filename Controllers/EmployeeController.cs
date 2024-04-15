using EllieApi.Dto;
using EllieApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EllieApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : GenericController
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
        public async Task<IActionResult> Create(EmployeeDto employeeDto)
        {
            Employee employee = new Employee();
            if (ModelState.IsValid)
            {
                CreatePasswordHash(employeeDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // Create a new Employee object and copy data from EmployeeDto
                employee = new Employee
                {
                    FirstName = employeeDto.FirstName,
                    LastName = employeeDto.LastName,
                    Email = employeeDto.Email,
                    InstituteId = employeeDto.InstituteId,
                    RoleId = employeeDto.RoleId,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                _context.Add(employee);
                await _context.SaveChangesAsync();
            }
            return StatusCode(201, employee);
        }
        
        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Dictionary<string, object> updates)
        {
            Employee Employee = await _context.Employees.FindAsync(id);

            if (Employee == null)
            {
                return NotFound();
            }

            foreach (var update in updates)
            {
                var field = Employee.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(update.Key, StringComparison.OrdinalIgnoreCase));
                if (field != null && field.CanWrite)
                {
                    if (update.Value == null)
                    {
                        field.SetValue(Employee, null);
                    }
                    else
                    {
                        field.SetValue(Employee, ChangeType(update.Value.ToString(), field.PropertyType));
                    }
                }
            }

            _context.Entry(Employee).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
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

        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
