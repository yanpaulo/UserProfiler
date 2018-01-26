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
    public class ContentPagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContentPagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContentPages
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContentPages.ToListAsync());
        }
        
        // GET: ContentPages/Details/5
        [Route("/p/{url}")]
        public async Task<IActionResult> Display(string url)
        {
            if (url == null)
            {
                return NotFound();
            }

            var contentPage = await _context.ContentPages
                .SingleOrDefaultAsync(m => m.Url.ToLower() == url.ToLower());
            if (contentPage == null)
            {
                return NotFound();
            }

            return View(contentPage);
        }

        // GET: ContentPages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContentPages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Url")] ContentPage contentPage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contentPage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contentPage);
        }

        // GET: ContentPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contentPage = await _context.ContentPages.SingleOrDefaultAsync(m => m.Id == id);
            if (contentPage == null)
            {
                return NotFound();
            }
            return View(contentPage);
        }

        // POST: ContentPages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Url")] ContentPage contentPage)
        {
            if (id != contentPage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contentPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentPageExists(contentPage.Id))
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
            return View(contentPage);
        }

        // GET: ContentPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contentPage = await _context.ContentPages
                .SingleOrDefaultAsync(m => m.Id == id);
            if (contentPage == null)
            {
                return NotFound();
            }

            return View(contentPage);
        }

        // POST: ContentPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contentPage = await _context.ContentPages.SingleOrDefaultAsync(m => m.Id == id);
            _context.ContentPages.Remove(contentPage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentPageExists(int id)
        {
            return _context.ContentPages.Any(e => e.Id == id);
        }
    }
}
