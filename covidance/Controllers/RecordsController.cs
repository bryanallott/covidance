using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using covidance.Data;
using covidance.Lib.Communications;
using Microsoft.Extensions.Logging;

namespace covidance.Controllers
{
    public class RecordsController : BaseAuthenticatedController
    {
        private readonly CovidanceContext _context;
        private IQueryable<RecordInfo> _defaultRecords;
        private readonly IMyEmailSender _emailSender;
        private readonly ILogger<RecordsController> _logger;
        public RecordsController(CovidanceContext context, ILogger<RecordsController> logger, IMyEmailSender emailSender)
        {
            _context = context;
            _defaultRecords = _context.Records.Where(x => x.PersonInfo.UserId == User.Identity.Name);
            _logger = logger;
            _emailSender = emailSender;
        }

        // GET: Records
        public async Task<IActionResult> Index()
        {
            return View(await _defaultRecords.ToListAsync());
        }

        // GET: Records/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recordInfo = await _defaultRecords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recordInfo == null)
            {
                return NotFound();
            }

            return View(recordInfo);
        }

        // GET: Records/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Records/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("When,Temperature,Symptoms,RecentContact,Sanitised,Bagged,Reason,Photo")] RecordInfo recordInfo)
        {
            if (ModelState.IsValid)
            {
                recordInfo.Id = Guid.NewGuid();
                recordInfo.DateCreated = DateTime.UtcNow;
                _context.Add(recordInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recordInfo);
        }

        // GET: Records/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recordInfo = await _defaultRecords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recordInfo == null)
            {
                return NotFound();
            }

            return View(recordInfo);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var recordInfo = await _defaultRecords.SingleOrDefaultAsync(x => x.Id == id);
            _context.Records.Remove(recordInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordInfoExists(Guid id)
        {
            return _defaultRecords.Any(e => e.Id == id);
        }
    }
}
