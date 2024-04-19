using EllieApi.Dto;
using EllieApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EllieApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : GenericController
    {
        private readonly ElliedbContext _context;
        private readonly IConfiguration _configuration;

        public EmployeeController(ElliedbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Employee
        [HttpGet, Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Employees.Include(b => b.Institute).Include(e => e.Institute.Address).ToListAsync());
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
                    Active = employeeDto.Active,
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

        private bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(hash);
            }
        }

        private string CreateToken(string email, string userType)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, userType)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(100),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private async Task<bool> CheckIfUserExistByEmail(string email, string password)
        {
            Employee userF = _context.Employees.Where(e => e.Email == email).FirstOrDefault();
            if (userF != null)
            {
                bool verified = VerifyPasswordHash(password, userF.PasswordHash, userF.PasswordSalt);
                if (verified)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost("UserLogin")]
        public async Task<IActionResult> LoginUser(LoginUserDto user)
        {
            bool loginSuccess;
            try
            {
                loginSuccess = await CheckIfUserExistByEmail(user.email, user.password);
            }
            catch (Exception e)
            {
                return StatusCode(404, "Username or password is wrong");
            }
            if (loginSuccess)
            {
                Employee userByEmail;
                try
                {
                    userByEmail = await _context.Employees.Where(c => c.Email == user.email)
                        .Include(b => b.Role)
                        .FirstOrDefaultAsync();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
                if (userByEmail.Active == true)
                {
                    LoginSuccessUserDto login = new LoginSuccessUserDto();
                    login.id = userByEmail.Id;
                    login.email = userByEmail.Email;
                    login.userType1 = userByEmail.Role.Name;
                    string token = CreateToken(user.email, userByEmail.Role.Name);
                    var obj = new { login, token };
                    return Ok(obj);
                }
                else
                {
                    return StatusCode(423, "User inactive");
                }
            }
            return StatusCode(418, "is Bed");
        }
    }
}
