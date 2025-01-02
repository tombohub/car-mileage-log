using CarMileageLog.Data;
using CarMileageLog.Data.Models;
using CarMileageLog.FormDataModels;
using CarMileageLog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace CarMileageLog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            var viewModel = new HomeIndexViewModel
            {
                StartDriveFormData = new StartDriveFormData(),
                EndDriveFormData = new EndDriveFormData(),
                JobSites = _context.JobSites
                    .Select(js => new SelectListItem(js.Name, js.Id.ToString()))
                    .ToList(),
                IsDriveInProgress = IsDriveInProgress(),
                Today = DateOnly.FromDateTime(DateTime.Now),
                DriveInProgress = GetDriveInProgress()
            };
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult StartDrive(StartDriveFormData startDriveFormData)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data!";
                return RedirectToAction(nameof(Index));
            }

            var newDriveLog = new DriveLog
            {
                Date = startDriveFormData.Date,
                StartKilometers = startDriveFormData.StartKilometers,
                JobSiteId = startDriveFormData.JobSiteId,
                Status = DriveStatus.InProgress
            };

            _context.DriveLogs.Add(newDriveLog);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Drive started!";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EndDrive(EndDriveFormData endDriveFormData)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data!";
                return RedirectToAction(nameof(Index));
            }

            var driveLog = _context.DriveLogs.Find(endDriveFormData.DriveLogId);

            if (driveLog == null)
            {
                TempData["ErrorMessage"] = "Drive not found!";
                return RedirectToAction(nameof(Index));
            }

            if (driveLog.StartKilometers > endDriveFormData.EndKilometers)
            {
                TempData["ErrorMessage"] = $"End kilometers {endDriveFormData.EndKilometers} must be greater than start kilometers!";
                return RedirectToAction(nameof(Index));
            }

            driveLog.EndKilometers = endDriveFormData.EndKilometers;
            driveLog.Status = DriveStatus.Completed;
            _context.DriveLogs.Update(driveLog);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Drive ended!";
            return RedirectToAction(nameof(Index));
        }

        private bool IsDriveInProgress()
        {
            return GetDriveInProgress() != null;
        }

        private DriveLog? GetDriveInProgress()
        {
            return _context.DriveLogs
                .Where(dl => dl.Status == DriveStatus.InProgress)
                .FirstOrDefault();
        }
    }
}
