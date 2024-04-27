using EllieApi.Models;
using EllieApi.Service.Interfaces;
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
    public class AddressController : GenericController
    {
        private IAddressRepository _repository;

        public AddressController(IAddressRepository repository)
        {
            _repository = repository;
        }

        // GET: Address
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("id")]
        // GET: Address/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Address = await _repository.GetById(id);
            if (Address == null)
            {
                return NotFound();
            }

            return Ok(Address);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Address Address)
        {
            if (ModelState.IsValid)
            {
                _repository.Insert(Address);
            }
            return StatusCode(201, Address);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Dictionary<string, object> updates)
        {
            if (!_repository.entityExists(id).Result)
            {
                return NotFound();
            }
            await _repository.Update(id, updates);
            
            return Ok(updates);
        }

        // POST: Address/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_repository.entityExists(id).Result)
            {
                return NotFound();
            }
            await _repository.Delete(id);
            return Ok(id);
        }
    }
}
