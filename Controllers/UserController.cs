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
using System.Threading.Tasks;

namespace EllieApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : GenericController
    {
        private readonly ElliedbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(ElliedbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("AppUserLogin")]
        public async Task<IActionResult> LoginApp(int roomId)
        {
            bool loginSuccess;
            try
            {
                loginSuccess = await CheckIfRoomHasUser(roomId);
                if (loginSuccess)
                {
                    var userId = _context.Rooms.FirstOrDefaultAsync(c => c.Id == roomId).Result.UserId;
                    User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    AppLoginDto userData = new AppLoginDto();
                    userData.FirstName = user.FirstName; 
                    userData.LastName = user.LastName;
                    userData.Points = user.Points;
                    userData.Id = user.Id;
                    string token = CreateToken(userData.FirstName, "Beboer");
                    userData.Token = token;
                    return Ok(userData);
                } else
                {
                    return StatusCode(404, "No user found in room");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Something went wrong");
            }
            return StatusCode(418, "is Bed");
        }

        // GET: User
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Users.Include(e => e.Notes).Include(a => a.ContactPerson).Include(e => e.Rooms).ToListAsync());
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

        [HttpPost, Authorize(Roles = "Admin, Pædagog")]
        public async Task<IActionResult> Create(User User)
        {
            if (ModelState.IsValid)
            {
                _context.Add(User);
                await _context.SaveChangesAsync();
            }
            return StatusCode(201, User);
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> Edit(int id, [FromBody] Dictionary<string, object> updates)
        {
            User User = await _context.Users.FindAsync(id);

            if (User == null)
            {
                return NotFound();
            }

            foreach (var update in updates)
            {
                var field = User.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(update.Key, StringComparison.OrdinalIgnoreCase));
                if (field != null && field.CanWrite)
                {
                    if (update.Value == null)
                    {
                        field.SetValue(User, null);
                    }
                    else
                    {
                        field.SetValue(User, ChangeType(update.Value.ToString(), field.PropertyType));
                    }
                }
            }

            _context.Entry(User).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
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

        private async Task<bool> CheckIfRoomHasUser(int roomId)
        {
            Room room = await _context.Rooms.Where(c => c.Id == roomId).FirstOrDefaultAsync();

            if (room != null)
            {
                if(room.UserId != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
