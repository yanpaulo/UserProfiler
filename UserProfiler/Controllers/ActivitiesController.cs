using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserProfiler.Models;

namespace UserProfiler.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Activities
        [Route("/user/{userId}/activity")]
        [Route("/page/{pageId}/activity")]
        public async Task<IActionResult> Index(Guid? userId, int? pageId)
        {
            var applicationDbContext = _context.UserActivities.Include(u => u.AnonymousUser).Include(u => u.ContentPage)
                .Where(a => a.AnonymousUserId == userId || a.ContentPageId == pageId)
                .OrderByDescending(a => a.Date);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userActivity = await _context.UserActivities
                .Include(u => u.AnonymousUser)
                .Include(u => u.ContentPage)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (userActivity == null)
            {
                return NotFound();
            }

            return View(userActivity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            ViewData["AnonymousUserId"] = new SelectList(_context.AnonymousUsers, "Id", "AppVersion");
            ViewData["ContentPageId"] = new SelectList(_context.ContentPages, "Id", "Title");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kind,Coordinates,Date,AnonymousUserId,ContentPageId")] UserActivity userActivity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userActivity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnonymousUserId"] = new SelectList(_context.AnonymousUsers, "Id", "AppVersion", userActivity.AnonymousUserId);
            ViewData["ContentPageId"] = new SelectList(_context.ContentPages, "Id", "Title", userActivity.ContentPageId);
            return View(userActivity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userActivity = await _context.UserActivities.SingleOrDefaultAsync(m => m.Id == id);
            if (userActivity == null)
            {
                return NotFound();
            }
            ViewData["AnonymousUserId"] = new SelectList(_context.AnonymousUsers, "Id", "AppVersion", userActivity.AnonymousUserId);
            ViewData["ContentPageId"] = new SelectList(_context.ContentPages, "Id", "Title", userActivity.ContentPageId);
            return View(userActivity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kind,Coordinates,Date,AnonymousUserId,ContentPageId")] UserActivity userActivity)
        {
            if (id != userActivity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userActivity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserActivityExists(userActivity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnonymousUserId"] = new SelectList(_context.AnonymousUsers, "Id", "AppVersion", userActivity.AnonymousUserId);
            ViewData["ContentPageId"] = new SelectList(_context.ContentPages, "Id", "Title", userActivity.ContentPageId);
            return View(userActivity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userActivity = await _context.UserActivities
                .Include(u => u.AnonymousUser)
                .Include(u => u.ContentPage)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (userActivity == null)
            {
                return NotFound();
            }

            return View(userActivity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userActivity = await _context.UserActivities.SingleOrDefaultAsync(m => m.Id == id);
            _context.UserActivities.Remove(userActivity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserActivityExists(int id)
        {
            return _context.UserActivities.Any(e => e.Id == id);
        }
    }
}
