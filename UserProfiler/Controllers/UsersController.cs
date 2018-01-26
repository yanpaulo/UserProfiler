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
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnonymousUsers.OrderByDescending(u => u.CreationDate).ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anonymousUser = await _context.AnonymousUsers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (anonymousUser == null)
            {
                return NotFound();
            }

            return View(anonymousUser);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppVersion,UserAgent,CreationDate")] AnonymousUser anonymousUser)
        {
            if (ModelState.IsValid)
            {
                anonymousUser.Id = Guid.NewGuid();
                _context.Add(anonymousUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(anonymousUser);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anonymousUser = await _context.AnonymousUsers.SingleOrDefaultAsync(m => m.Id == id);
            if (anonymousUser == null)
            {
                return NotFound();
            }
            return View(anonymousUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,AppVersion,UserAgent,CreationDate")] AnonymousUser anonymousUser)
        {
            if (id != anonymousUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(anonymousUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnonymousUserExists(anonymousUser.Id))
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
            return View(anonymousUser);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anonymousUser = await _context.AnonymousUsers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (anonymousUser == null)
            {
                return NotFound();
            }

            return View(anonymousUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var anonymousUser = await _context.AnonymousUsers.SingleOrDefaultAsync(m => m.Id == id);
            _context.AnonymousUsers.Remove(anonymousUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnonymousUserExists(Guid id)
        {
            return _context.AnonymousUsers.Any(e => e.Id == id);
        }
    }
}
