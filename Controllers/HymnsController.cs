using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HymnHub.Data;
using HymnHub.Models;

namespace HymnHub.Controllers
{
    public class HymnsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public HymnsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // ... Other actions

        // POST: Hymns/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Lyrics,MusicSheetUrl,InstrumentalMusicUrl")] Hymn hymn, IFormFile musicSheetFile, IFormFile instrumentalMusicFile)
        {
            if (ModelState.IsValid)
            {
                if (musicSheetFile != null && musicSheetFile.Length > 0)
                {
                    string musicSheetFileName = Path.GetFileName(musicSheetFile.FileName);
                    string musicSheetPath = Path.Combine(_environment.WebRootPath, "uploads", "music_sheets", musicSheetFileName);

                    using (var stream = new FileStream(musicSheetPath, FileMode.Create))
                    {
                        await musicSheetFile.CopyToAsync(stream);
                    }

                    hymn.MusicSheetUrl = musicSheetFileName;
                }

                if (instrumentalMusicFile != null && instrumentalMusicFile.Length > 0)
                {
                    string instrumentalMusicFileName = Path.GetFileName(instrumentalMusicFile.FileName);
                    string instrumentalMusicPath = Path.Combine(_environment.WebRootPath, "uploads", "instrumental_music", instrumentalMusicFileName);

                    using (var stream = new FileStream(instrumentalMusicPath, FileMode.Create))
                    {
                        await instrumentalMusicFile.CopyToAsync(stream);
                    }

                    hymn.InstrumentalMusicUrl = instrumentalMusicFileName;
                }

                _context.Add(hymn);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(hymn);
        }

        // POST: Hymns/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Lyrics,MusicSheetUrl,InstrumentalMusicUrl")] Hymn hymn, IFormFile musicSheetFile, IFormFile instrumentalMusicFile)
        {
            if (id != hymn.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (musicSheetFile != null && musicSheetFile.Length > 0)
                    {
                        string musicSheetFileName = Path.GetFileName(musicSheetFile.FileName);
                        string musicSheetPath = Path.Combine(_environment.WebRootPath, "uploads", "music_sheets", musicSheetFileName);

                        using (var stream = new FileStream(musicSheetPath, FileMode.Create))
                        {
                            await musicSheetFile.CopyToAsync(stream);
                        }

                        hymn.MusicSheetUrl = musicSheetFileName;
                    }

                    if (instrumentalMusicFile != null && instrumentalMusicFile.Length > 0)
                    {
                        string instrumentalMusicFileName = Path.GetFileName(instrumentalMusicFile.FileName);
                        string instrumentalMusicPath = Path.Combine(_environment.WebRootPath, "uploads", "instrumental_music", instrumentalMusicFileName);

                        using (var stream = new FileStream(instrumentalMusicPath, FileMode.Create))
                        {
                            await instrumentalMusicFile.CopyToAsync(stream);
                        }

                        hymn.InstrumentalMusicUrl = instrumentalMusicFileName;
                    }

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

        // ... Other actions

        private bool HymnExists(int id)
        {
            return (_context.Hymn?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
