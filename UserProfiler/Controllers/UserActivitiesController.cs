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
    [Route("api/UserActivities")]
    public class UserActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserActivities
        [HttpGet]
        public IEnumerable<UserActivity> GetUserActivities()
        {
            return _context.UserActivities;
        }

        // GET: api/UserActivities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserActivity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userActivity = await _context.UserActivities.SingleOrDefaultAsync(m => m.Id == id);

            if (userActivity == null)
            {
                return NotFound();
            }

            return Ok(userActivity);
        }

        // POST: api/UserActivities
        [HttpPost]
        public async Task<IActionResult> PostUserActivity([FromBody] UserActivity userActivity)
        {
            ModelState.Remove("ContentPage.Title");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var url = userActivity.ContentPage.Url;
            url = url.Remove(0, url.LastIndexOf("/") + 1);
            var page = _context.ContentPages.FirstOrDefault(p => p.Url.ToLower() == url.ToLower());

            if (page == null)
            {
                ModelState.AddModelError("ContentPage.Url", "Página Inexistente");
                return BadRequest(ModelState);
            }
            if (!_context.AnonymousUsers.Any(u => u.Id == userActivity.AnonymousUserId))
            {
                ModelState.AddModelError("AnonymousUserId", "Usuário Inexistente");
                return BadRequest(ModelState);
            }


            userActivity.ContentPage = page;
            userActivity.Date = DateTimeOffset.UtcNow;

            _context.UserActivities.Add(userActivity);
            await _context.SaveChangesAsync();

            return Ok();
        }


        // PUT: api/UserActivities/5
        [HttpPut("{id}")]
        [NonAction]
        public async Task<IActionResult> PutUserActivity([FromRoute] int id, [FromBody] UserActivity userActivity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userActivity.Id)
            {
                return BadRequest();
            }

            _context.Entry(userActivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserActivityExists(id))
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


        // DELETE: api/UserActivities/5
        [HttpDelete("{id}")]
        [NonAction]
        public async Task<IActionResult> DeleteUserActivity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userActivity = await _context.UserActivities.SingleOrDefaultAsync(m => m.Id == id);
            if (userActivity == null)
            {
                return NotFound();
            }

            _context.UserActivities.Remove(userActivity);
            await _context.SaveChangesAsync();

            return Ok(userActivity);
        }

        private bool UserActivityExists(int id)
        {
            return _context.UserActivities.Any(e => e.Id == id);
        }
    }
}