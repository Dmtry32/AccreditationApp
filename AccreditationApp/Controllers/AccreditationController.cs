//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using AccreditationApp.Models;
//using System.Collections.Generic;
//using System.Linq;

//namespace AccreditationApp.Controllers
//{
//    [Authorize]
//    public class AccreditationController : Controller
//    {
//        private static List<AccreditationModel> _accreditations = new List<AccreditationModel>();
//        private static int _nextId = 1;

//        public IActionResult Index()
//        {
//            return RedirectToAction("Create");
//        }

//        [HttpGet]
//        public IActionResult Create()
//        {
//            ViewBag.Banks = new List<string>
//            {
//                "Crédit Agricole",
//                "BNP Paribas",
//                "Société Générale",
//                "Banque Populaire",
//                "Caisse d'Épargne",
//                "Crédit Mutuel",
//                "La Banque Postale",
//                "HSBC France",
//                "LCL"
//            };

//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Create(AccreditationModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                model.Id = _nextId++;
//                model.Status = AccreditationStatus.Pending;
//                model.RequestDate = DateTime.Now;

//                _accreditations.Add(model);

//                return RedirectToAction("Current");
//            }

//            ViewBag.Banks = new List<string>
//            {
//                "Crédit Agricole",
//                "BNP Paribas",
//                "Société Générale",
//                "Banque Populaire",
//                "Caisse d'Épargne",
//                "Crédit Mutuel",
//                "La Banque Postale",
//                "HSBC France",
//                "LCL"
//            };

//            return View(model);
//        }

//        public IActionResult Current()
//        {
//            return View(_accreditations.Where(a => a.Status == AccreditationStatus.Pending).ToList());
//        }

//        public IActionResult History()
//        {
//            return View(_accreditations.Where(a => a.Status != AccreditationStatus.Pending).ToList());
//        }
//    }
//}

//Fix version 
using AccreditationApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Remove the problematic using statement and use the correct namespace
namespace AccreditationApp.Controllers
{
    [Authorize]
    public class AccreditationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccreditationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Accreditation/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Banks = new List<string>
            {
                "BNP Paribas",
                "Crédit Agricole",
                "Société Générale",
                "LCL",
                "HSBC",
                "Banque Populaire",
                "Caisse d'Épargne",
                "Autre"
            };

            return View();
        }

        // POST: /Accreditation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccreditationModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var accreditation = new Accreditation
                    {
                        LastName = model.LastName,
                        FirstName = model.FirstName,
                        Email = model.Email,
                        BirthDate = model.BirthDate,
                        Bank = model.Bank,
                        Status = "En attente",
                        RequestDate = System.DateTime.Now
                    };

                    _context.Accreditations.Add(accreditation);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Accréditation créée avec succès!";
                    return RedirectToAction("Index", "Home");
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", "Une erreur s'est produite lors de l'enregistrement: " + ex.Message);
                }
            }

            ViewBag.Banks = new List<string>
            {
                "BNP Paribas", "Crédit Agricole", "Société Générale", "LCL",
                "HSBC", "Banque Populaire", "Caisse d'Épargne", "Autre"
            };

            return View(model);
        }

        // GET: /Accreditation/Current
        public IActionResult Current()
        {
            // For now, use a simple implementation without async
            var accreditations = _context.Accreditations
                .Where(a => a.Status == "En attente" || a.Status == "En cours")
                .OrderByDescending(a => a.RequestDate)
                .ToList();

            return View(accreditations);
        }

        // GET: /Accreditation/History
        public IActionResult History()
        {
            var accreditations = _context.Accreditations.ToList();
            return View(accreditations);
        }
    }
}