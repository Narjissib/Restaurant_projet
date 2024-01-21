using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myapp.Data;
using myapp.Models;


namespace projetnet.Controllers
{
    public class PlatsController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public PlatsController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            webHostEnvironment = webHost;
        }

        // GET: Plats
        public IActionResult Index()
        {
            // Remplir le modèle avec les données de la base de données
            var model = _context.Plat.Where(p => p.CategorieId == 1).ToList();
            Console.WriteLine($"Nombre de plats récupérés depuis la base de données : {model.Count}");

            // Renvoyer le modèle à la vue
            return View("Index", model);
        }
        public IActionResult Index1()
        {
            // Remplir le modèle avec les données de la base de données
            var model = _context.Plat.Where(p => p.CategorieId == 2).ToList();
            Console.WriteLine($"Nombre de plats récupérés depuis la base de données : {model.Count}");

            // Renvoyer le modèle à la vue
            return View("Index1", model);
        }
        public IActionResult Index2()
        {
            // Remplir le modèle avec les données de la base de données
            var model = _context.Plat.Where(p => p.CategorieId == 3).ToList();
            Console.WriteLine($"Nombre de plats récupérés depuis la base de données : {model.Count}");

            // Renvoyer le modèle à la vue
            return View("Index2", model);
        }
        public IActionResult Index3()
        {
            // Remplir le modèle avec les données de la base de données
            var model = _context.Plat.Where(p => p.CategorieId == 4).ToList();
            Console.WriteLine($"Nombre de plats récupérés depuis la base de données : {model.Count}");

            // Renvoyer le modèle à la vue
            return View("Index3", model);
        }
        public IActionResult Index4()
        {
            // Remplir le modèle avec les données de la base de données
            var model = _context.Plat.Where(p => p.CategorieId == 5).ToList();
            Console.WriteLine($"Nombre de plats récupérés depuis la base de données : {model.Count}");

            // Renvoyer le modèle à la vue
            return View("Index4", model);
        }
        public IActionResult Index5()
        {
            // Remplir le modèle avec les données de la base de données
            var model = _context.Plat.Where(p => p.CategorieId == 6).ToList();
            Console.WriteLine($"Nombre de plats récupérés depuis la base de données : {model.Count}");

            // Renvoyer le modèle à la vue
            return View("Index5", model);
        }
        public IActionResult Index6()
        {
            // Remplir le modèle avec les données de la base de données
            var model = _context.Plat.Where(p => p.CategorieId == 7).ToList();
            Console.WriteLine($"Nombre de plats récupérés depuis la base de données : {model.Count}");

            // Renvoyer le modèle à la vue
            return View("Index6", model);
        }
        public IActionResult Index7()
        {
            // Remplir le modèle avec les données de la base de données
            var model = _context.Plat.Where(p => p.CategorieId == 8).ToList();
            Console.WriteLine($"Nombre de plats récupérés depuis la base de données : {model.Count}");

            // Renvoyer le modèle à la vue
            return View("Index7", model);
        }


        // POST: Plats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.



        // GET: Plats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Plat plat = await _context.Plat.Where(x => x.PlatId == id).FirstOrDefaultAsync();

            if (plat == null)
            {
                return NotFound();
            }

            return View(plat);
        }
        public async Task<IActionResult> Details1(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Plat plat = await _context.Plat.Where(x => x.PlatId == id).FirstOrDefaultAsync();

            if (plat == null)
            {
                return NotFound();
            }

            return View(plat);
        }
        public async Task<IActionResult> Details2(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Plat plat = await _context.Plat.Where(x => x.PlatId == id).FirstOrDefaultAsync();

            if (plat == null)
            {
                return NotFound();
            }

            return View(plat);
        }
        public async Task<IActionResult> Details3(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Plat plat = await _context.Plat.Where(x => x.PlatId == id).FirstOrDefaultAsync();

            if (plat == null)
            {
                return NotFound();
            }

            return View(plat);
        }
        public async Task<IActionResult> Details4(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Plat plat = await _context.Plat.Where(x => x.PlatId == id).FirstOrDefaultAsync();

            if (plat == null)
            {
                return NotFound();
            }

            return View(plat);
        }
        public async Task<IActionResult> Details5(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Plat plat = await _context.Plat.Where(x => x.PlatId == id).FirstOrDefaultAsync();

            if (plat == null)
            {
                return NotFound();
            }

            return View(plat);
        }
        public async Task<IActionResult> Details6(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Plat plat = await _context.Plat.Where(x => x.PlatId == id).FirstOrDefaultAsync();

            if (plat == null)
            {
                return NotFound();
            }

            return View(plat);
        }
        public async Task<IActionResult> Details7(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Plat plat = await _context.Plat.Where(x => x.PlatId == id).FirstOrDefaultAsync();

            if (plat == null)
            {
                return NotFound();
            }

            return View(plat);
        }

        public IActionResult Consulter(int categorieId)
        {
            // Utilisez un switch pour déterminer la logique en fonction de la catégorie
            switch (categorieId)
            {
                case 1:
                    // Logique spécifique à la catégorie 15
                    var plats = _context.Plat.Where(p => p.CategorieId == 1).ToList();
                    // Vous pouvez également ajouter d'autres logiques de traitement ici si nécessaire

                    // Renvoyer les plats à la vue
                    return View("Index", plats);
                case 2:
                    // Logique spécifique à la catégorie 16
                    var plats16 = _context.Plat.Where(p => p.CategorieId == 2).ToList();
                    return View("Index1", plats16);
                case 3:
                    // Logique spécifique à la catégorie 17
                    var plats17 = _context.Plat.Where(p => p.CategorieId == 3).ToList();
                    return View("Index2", plats17);
                case 4:
                    // Logique spécifique à la catégorie 18
                    var plats18 = _context.Plat.Where(p => p.CategorieId == 4).ToList();
                    return View("Index3", plats18);
                case 5:
                    // Logique spécifique à la catégorie 19
                    var plats19 = _context.Plat.Where(p => p.CategorieId == 5).ToList();
                    return View("Index4", plats19);
                case 6:
                    // Logique spécifique à la catégorie 18
                    var plats20 = _context.Plat.Where(p => p.CategorieId == 6).ToList();
                    return View("Index5", plats20);
                case 7:
                    // Logique spécifique à la catégorie 19
                    var plats21 = _context.Plat.Where(p => p.CategorieId == 7).ToList();
                    return View("Index6", plats21);
                case 8:
                    // Logique spécifique à la catégorie 18
                    var plats22 = _context.Plat.Where(p => p.CategorieId == 8).ToList();
                    return View("Index7", plats22);
                
                default:
                    // Logique par défaut (si la catégorie n'est pas gérée)
                    return NotFound(); // Ou une vue d'erreur par exemple
            }
        }





        

        // GET: Plats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plat plat)
        {
            string uniqueFileName = UploadedFile(plat);
            plat.ImgUrl = uniqueFileName;
            _context.Attach(plat);
            _context.Entry(plat).State = EntityState.Added;
            //_context.Add(categorie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Create1()
        {
            return View();
        }

        // POST: Plats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create1(Plat plat)
        {
            string uniqueFileName = UploadedFile(plat);
            plat.ImgUrl = uniqueFileName;
            _context.Attach(plat);
            _context.Entry(plat).State = EntityState.Added;
            await _context.SaveChangesAsync();

            // Rediriger explicitement vers Index1 dans le contrôleur Plats
            return RedirectToAction(nameof(Index1));
        }
        public IActionResult Create2()
        {
            return View();
        }

        // POST: Plats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2(Plat plat)
        {
            string uniqueFileName = UploadedFile(plat);
            plat.ImgUrl = uniqueFileName;
            _context.Attach(plat);
            _context.Entry(plat).State = EntityState.Added;
            await _context.SaveChangesAsync();

            // Rediriger explicitement vers Index1 dans le contrôleur Plats
            return RedirectToAction(nameof(Index2));
        }
        public IActionResult Create3()
        {
            return View();
        }

        // POST: Plats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create3(Plat plat)
        {
            string uniqueFileName = UploadedFile(plat);
            plat.ImgUrl = uniqueFileName;
            _context.Attach(plat);
            _context.Entry(plat).State = EntityState.Added;
            await _context.SaveChangesAsync();

            // Rediriger explicitement vers Index1 dans le contrôleur Plats
            return RedirectToAction(nameof(Index3));
        }
        public IActionResult Create4()
        {
            return View();
        }

        // POST: Plats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create4(Plat plat)
        {
            string uniqueFileName = UploadedFile(plat);
            plat.ImgUrl = uniqueFileName;
            _context.Attach(plat);
            _context.Entry(plat).State = EntityState.Added;
            await _context.SaveChangesAsync();

            // Rediriger explicitement vers Index1 dans le contrôleur Plats
            return RedirectToAction(nameof(Index4));
        }
        public IActionResult Create5()
        {
            return View();
        }

        // POST: Plats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create5(Plat plat)
        {
            string uniqueFileName = UploadedFile(plat);
            plat.ImgUrl = uniqueFileName;
            _context.Attach(plat);
            _context.Entry(plat).State = EntityState.Added;
            await _context.SaveChangesAsync();

            // Rediriger explicitement vers Index1 dans le contrôleur Plats
            return RedirectToAction(nameof(Index5));
        }
        public IActionResult Create6()
        {
            return View();
        }

        // POST: Plats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create6(Plat plat)
        {
            string uniqueFileName = UploadedFile(plat);
            plat.ImgUrl = uniqueFileName;
            _context.Attach(plat);
            _context.Entry(plat).State = EntityState.Added;
            await _context.SaveChangesAsync();

            // Rediriger explicitement vers Index1 dans le contrôleur Plats
            return RedirectToAction(nameof(Index6));
        }
        public IActionResult Create7()
        {
            return View();
        }

        // POST: Plats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create7(Plat plat)
        {
            string uniqueFileName = UploadedFile(plat);
            plat.ImgUrl = uniqueFileName;
            _context.Attach(plat);
            _context.Entry(plat).State = EntityState.Added;
            await _context.SaveChangesAsync();

            // Rediriger explicitement vers Index1 dans le contrôleur Plats
            return RedirectToAction(nameof(Index7));
        }



        private string UploadedFile(Plat plat)
        {
            string uniqueFileName = null;
            if (plat.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + plat.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    plat.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platToDelete = await _context.Plat.FindAsync(id);

            if (platToDelete == null)
            {
                return NotFound();
            }

            return View(platToDelete);
        }

        // Action pour effectuer la suppression
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var platToDelete = await _context.Plat.FindAsync(id);

            if (platToDelete == null)
            {
                return NotFound();
            }

            // Supprimez l'élément de la base de données
            _context.Plat.Remove(platToDelete);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
