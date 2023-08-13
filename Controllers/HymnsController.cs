using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HymnHub.Data;
using HymnHub.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace HymnHub.Controllers
{
    public class HymnsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HymnsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // search
        public async Task<IActionResult> Index(string searchString)
        {
            var movies = from m in _context.Hymn
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title!.Contains(searchString));
            }

            if (movies.ToList().IsNullOrEmpty())
            {
                return View("EmptySearch");
            }
            
            return View(await movies.ToListAsync());
        }

        // GET: Hymns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hymn == null)
            {
                return NotFound();
            }

            var hymn = await _context.Hymn
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hymn == null)
            {
                return NotFound();
            }

            return View(hymn);
        }

        // GET: Hymns/Create
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hymns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Lyrics,MusicSheetUrl,InstrumentalMusicUrl")] Hymn hymn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hymn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hymn);
        }

        // GET: Hymns/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hymn == null)
            {
                return NotFound();
            }

            var hymn = await _context.Hymn.FindAsync(id);
            if (hymn == null)
            {
                return NotFound();
            }
            return View(hymn);
        }

        // POST: Hymns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Lyrics,MusicSheetUrl,InstrumentalMusicUrl")] Hymn hymn)
        {
            if (id != hymn.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hymn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HymnExists(hymn.Id))
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
            return View(hymn);
        }

        // GET: Hymns/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hymn == null)
            {
                return NotFound();
            }

            var hymn = await _context.Hymn
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hymn == null)
            {
                return NotFound();
            }

            return View(hymn);
        }

        // POST: Hymns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hymn == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Hymn'  is null.");
            }
            var hymn = await _context.Hymn.FindAsync(id);
            if (hymn != null)
            {
                _context.Hymn.Remove(hymn);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HymnExists(int id)
        {
          return (_context.Hymn?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
