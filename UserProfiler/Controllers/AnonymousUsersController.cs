using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProfiler.Models;

namespace UserProfiler.Controllers
{
    [Produces("application/json")]
    [Route("api/AnonymousUsers")]
    public class AnonymousUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnonymousUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AnonymousUsers
        [HttpGet]
        public IEnumerable<AnonymousUser> GetAnonymousUsers()
        {
            return _context.AnonymousUsers;
        }

        // GET: api/AnonymousUsers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnonymousUser([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var anonymousUser = await _context.AnonymousUsers.SingleOrDefaultAsync(m => m.Id == id);

            if (anonymousUser == null)
            {
                return NotFound();
            }

            return Ok(anonymousUser);
        }
        
        // POST: api/AnonymousUsers
        [HttpPost]
        public async Task<IActionResult> PostAnonymousUser([FromBody] AnonymousUser anonymousUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            anonymousUser.CreationDate = DateTimeOffset.UtcNow;

            _context.AnonymousUsers.Add(anonymousUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnonymousUser", new { id = anonymousUser.Id }, anonymousUser);
        }

        // PUT: api/AnonymousUsers/5
        [HttpPut("{id}")]
        [NonAction]
        public async Task<IActionResult> PutAnonymousUser([FromRoute] Guid id, [FromBody] AnonymousUser anonymousUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != anonymousUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(anonymousUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnonymousUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/AnonymousUsers/5
        [HttpDelete("{id}")]
        [NonAction]
        public async Task<IActionResult> DeleteAnonymousUser([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var anonymousUser = await _context.AnonymousUsers.SingleOrDefaultAsync(m => m.Id == id);
            if (anonymousUser == null)
            {
                return NotFound();
            }

            _context.AnonymousUsers.Remove(anonymousUser);
            await _context.SaveChangesAsync();

            return Ok(anonymousUser);
        }

        private bool AnonymousUserExists(Guid id)
        {
            return _context.AnonymousUsers.Any(e => e.Id == id);
        }
    }
}