using CarMileageLog.Data;
using CarMileageLog.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarMileageLog.Controllers
{
    [Authorize]
    public class JobSitesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobSitesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var allJobSites = _context.JobSites
                .OrderByDescending(x => x.Id)
                .ToList();
            return View(allJobSites);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(JobSite jobSite)
        {
            if (ModelState.IsValid)
            {
                _context.JobSites.Add(jobSite);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Job Site added successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(jobSite);
        }

        public IActionResult Delete(int id)
        {
            var jobSite = _context.JobSites.Find(id);
            if (jobSite == null)
            {
                return NotFound();
            }
            return View(jobSite);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var jobSite = _context.JobSites.Find(id);
            if (jobSite == null)
            {
                return NotFound();
            }
            _context.JobSites.Remove(jobSite);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Job Site deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var jobSite = _context.JobSites.Find(id);
            if (jobSite == null)
            {
                return NotFound();
            }
            return View(jobSite);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(JobSite jobSite)
        {
            if (ModelState.IsValid)
            {
                _context.JobSites.Update(jobSite);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Job Site updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(jobSite);
        }
    }
}
