using EllieApi.Dto;
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
            return Ok(await _context.Alarms.Where(e => e.Active == true).OrderBy(a => a.ActivatingTime).ToListAsync());
        }

        [HttpGet("WithUsers")]
        public async Task<IActionResult> WithUsers()
        {
            List<Alarm> alarms = await _context.Alarms.Where(e => e.Active == true).OrderBy(a => a.ActivatingTime).Include(b => b.UserAlarmRelations).ToListAsync();
            List<AlarmWithUserDto> alarmWithUsers = new List<AlarmWithUserDto>();
            foreach (Alarm alarm in alarms)
            {
                User user = new User();
                foreach (UserAlarmRelation relation in alarm.UserAlarmRelations)
                {
                    user = await _context.Users.Where(e => e.Id == relation.UserId).FirstOrDefaultAsync();
                }
                alarmWithUsers.Add(new AlarmWithUserDto()
                {
                    Id = alarm.Id,
                    Name = alarm.Name,
                    ActivatingTime = alarm.ActivatingTime,
                    Active = alarm.Active,
                    Description = alarm.Description,
                    AlarmTypeId = alarm.AlarmTypeId,
                    ImageUrl = alarm.ImageUrl,
                    user = user
                }
                );
            }
            return Ok(alarmWithUsers);
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
        public async Task<IActionResult> Create(AlarmPostDto alarm)
        {
            if (ModelState.IsValid)
            {
                if (alarm.IsAllUsersChecked)
                {
                    List<User> users = new List<User>();
                    users = await _context.Users.Where(e => e.Active == true).ToListAsync();
                    foreach (var user in users)
                    {
                        Alarm alarmPost = new Alarm();
                        UserAlarmRelation relation = new UserAlarmRelation();

                        switch (alarm.ImageUrl)
                        {
                            case "1":
                                alarmPost.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/medicine.png";
                                break;
                            case "2":
                                alarmPost.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/breakfast.png";
                                break;
                            case "3":
                                alarmPost.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/breakfast.png";
                                break;
                            case "4":
                                alarmPost.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/breakfast.png";
                                break;
                            case "5":
                                alarmPost.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/therapy.png";
                                break;
                            case "6":
                                alarmPost.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/training.png";
                                break;
                            case "7":
                                alarmPost.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/study.png";
                                break;
                        }

                        alarmPost.Active = true;
                        alarmPost.ActivatingTime = alarm.ActivatingTime;
                        alarmPost.AlarmTypeId = alarm.AlarmTypeId;
                        alarmPost.Name = alarm.Name;
                        alarmPost.Description = alarm.Description;
                        _context.Add(alarmPost);
                        await _context.SaveChangesAsync();

                        var alarmContext = await _context.Alarms.OrderByDescending(a => a.Id).FirstOrDefaultAsync();

                        relation.AlarmsId = alarmContext.Id;
                        relation.UserId = user.Id;
                        _context.Add(relation);
                        await _context.SaveChangesAsync();
                    }
                }
                else if (alarm.UserIds.Length > 0)
                {
                    foreach (var user in alarm.UserIds)
                    {
                        Alarm alarmPost = new Alarm();
                        UserAlarmRelation relation = new UserAlarmRelation();

                        switch (alarm.ImageUrl)
                        {
                            case "1":
                                alarmPost.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/medicine.png";
                                break;
                            case "2":
                                alarmPost.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/breakfast.png";
                                break;
                            case "3":
                                alarmPost.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/breakfast.png";
                                break;
                            case "4":
                                alarmPost.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/breakfast.png";
                                break;
                            case "5":
                                alarmPost.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/therapy.png";
                                break;
                            case "6":
                                alarmPost.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/training.png";
                                break;
                            case "7":
                                alarmPost.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/study.png";
                                break;
                            case "8":
                                alarm.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/study.png";
                                break;
                        }

                        alarmPost.Active = true;
                        alarmPost.ActivatingTime = alarm.ActivatingTime;
                        alarmPost.AlarmTypeId = alarm.AlarmTypeId;
                        alarmPost.Name = alarm.Name;
                        alarmPost.Description = alarm.Description;
                        _context.Add(alarmPost);
                        await _context.SaveChangesAsync();

                        var alarmContext = await _context.Alarms.OrderByDescending(a => a.Id).FirstOrDefaultAsync();

                        relation.AlarmsId = alarmContext.Id;
                        relation.UserId = user;
                        _context.Add(relation);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    return StatusCode(403, "Pick a user");
                }

            }
            return StatusCode(201, "Created");
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
                    if (update.Value == null)
                    {
                        field.SetValue(alarm, null);
                    }
                    else
                    {
                        field.SetValue(alarm, ChangeType(update.Value.ToString(), field.PropertyType));
                    }
                }
            }

            switch (alarm.ImageUrl)
            {
                case "1":
                    alarm.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/medicine.png";
                    break;
                case "2":
                    alarm.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/breakfast.png";
                    break;
                case "3":
                    alarm.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/breakfast.png";
                    break;
                case "4":
                    alarm.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/breakfast.png";
                    break;
                case "5":
                    alarm.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/therapy.png";
                    break;
                case "6":
                    alarm.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/training.png";
                    break;
                case "7":
                    alarm.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/study.png";
                    break;
                case "8":
                    alarm.ImageUrl = "https://ultimate-manually-chipmunk.ngrok-free.app/study.png";
                    break;
                default:
                    alarm.ImageUrl = alarm.ImageUrl;
                    break;
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
