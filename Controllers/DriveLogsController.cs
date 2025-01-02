using CarMileageLog.Data;
using CarMileageLog.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarMileageLog.Controllers
{
    public class DriveLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DriveLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DriveLogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DriveLogs.Include(d => d.JobSite);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DriveLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driveLog = await _context.DriveLogs
                .Include(d => d.JobSite)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driveLog == null)
            {
                return NotFound();
            }

            return View(driveLog);
        }

        // GET: DriveLogs/Create
        public IActionResult Create()
        {
            ViewData["JobSiteId"] = new SelectList(_context.JobSites, "Id", "Address");
            return View();
        }

        // POST: DriveLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,StartKilometers,EndKilometers,JobSiteId")] DriveLog driveLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(driveLog);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Drive log created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobSiteId"] = new SelectList(_context.JobSites, "Id", "Id", driveLog.JobSiteId);
            return View(driveLog);
        }

        // GET: DriveLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driveLog = await _context.DriveLogs.FindAsync(id);
            if (driveLog == null)
            {
                return NotFound();
            }
            ViewData["JobSiteId"] = new SelectList(_context.JobSites, "Id", "Address", driveLog.JobSiteId);
            return View(driveLog);
        }

        // POST: DriveLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,StartKilometers,EndKilometers,JobSiteId")] DriveLog driveLog)
        {
            if (id != driveLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driveLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriveLogExists(driveLog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessMessage"] = "Drive log updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobSiteId"] = new SelectList(_context.JobSites, "Id", "Id", driveLog.JobSiteId);
            return View(driveLog);
        }

        // GET: DriveLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driveLog = await _context.DriveLogs
                .Include(d => d.JobSite)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driveLog == null)
            {
                return NotFound();
            }


            return View(driveLog);
        }

        // POST: DriveLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driveLog = await _context.DriveLogs.FindAsync(id);
            if (driveLog != null)
            {
                _context.DriveLogs.Remove(driveLog);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Drive log deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool DriveLogExists(int id)
        {
            return _context.DriveLogs.Any(e => e.Id == id);
        }
    }
}
