using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myapp.Data;
using myapp.Models;

namespace myapp.Controllers
{
    public class PlatSignaturesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlatSignaturesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlatSignatures
        public async Task<IActionResult> Index()
        {
              return _context.PlatSignature != null ? 
                          View(await _context.PlatSignature.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.PlatSignature'  is null.");
        }

        // GET: PlatSignatures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PlatSignature == null)
            {
                return NotFound();
            }

            var platSignature = await _context.PlatSignature
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platSignature == null)
            {
                return NotFound();
            }

            return View(platSignature);
        }

        // GET: PlatSignatures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlatSignatures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Description")] PlatSignature platSignature)
        {
            if (ModelState.IsValid)
            {
                _context.Add(platSignature);
                await _context.SaveChangesAsync();
                return View("Confirmation");
            }
            return View(platSignature);
        }

        // GET: PlatSignatures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PlatSignature == null)
            {
                return NotFound();
            }

            var platSignature = await _context.PlatSignature.FindAsync(id);
            if (platSignature == null)
            {
                return NotFound();
            }
            return View(platSignature);
        }

        // POST: PlatSignatures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Description")] PlatSignature platSignature)
        {
            if (id != platSignature.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(platSignature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlatSignatureExists(platSignature.Id))
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
            return View(platSignature);
        }

        // GET: PlatSignatures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PlatSignature == null)
            {
                return NotFound();
            }

            var platSignature = await _context.PlatSignature
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platSignature == null)
            {
                return NotFound();
            }

            return View(platSignature);
        }

        // POST: PlatSignatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PlatSignature == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PlatSignature'  is null.");
            }
            var platSignature = await _context.PlatSignature.FindAsync(id);
            if (platSignature != null)
            {
                _context.PlatSignature.Remove(platSignature);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlatSignatureExists(int id)
        {
          return (_context.PlatSignature?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
