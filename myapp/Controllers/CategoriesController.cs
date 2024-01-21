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
    public class CategoriesController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            webHostEnvironment = webHost;
        }






        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return _context.Categorie != null ?
                        View(await _context.Categorie.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
        }

        // GET: Categories/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Categorie categorie = await _context.Categorie.Where(x => x.CategorieId == id).FirstOrDefaultAsync();

            if (categorie == null)
            {
                return NotFound();
            }

            return View(categorie);
        }


        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(Categorie categorie)
        {

            string uniqueFileName = UploadedFile(categorie);
            categorie.ImgUrl = uniqueFileName;
            _context.Attach(categorie);
            _context.Entry(categorie).State = EntityState.Added;
            //_context.Add(categorie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        private string UploadedFile(Categorie categorie)
        {
            string uniqueFileName = null;
            if (categorie.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + categorie.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    categorie.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;

        }

    }
}
