using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using covidance.Data;
using Microsoft.Extensions.Logging;
using covidance.Lib.Communications;

namespace covidance.Controllers
{
    
    public class PersonsController : BaseAuthenticatedController
    {
        private readonly CovidanceContext _context;
        private IQueryable<PersonInfo> _defaultPersons;
        private readonly IMyEmailSender _emailSender;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(CovidanceContext context, ILogger<PersonsController> logger, IMyEmailSender emailSender)
        {
            _context = context;
            _defaultPersons = _context.Persons.Where(x => x.UserId == User.Identity.Name);
            _logger = logger;
            _emailSender = emailSender;
        }

        // GET: Persons
        public async Task<IActionResult> Index()
        {
            return View(await _defaultPersons.ToListAsync());
        }

        // GET: Persons/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInfo = await _defaultPersons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personInfo == null)
            {
                return NotFound();
            }

            return View(personInfo);
        }

        // GET: Persons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Persons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email")] PersonInfo personInfo)
        {
            if (ModelState.IsValid)
            {
                personInfo.Id = Guid.NewGuid();
                personInfo.UserId = User.Identity.Name;
                _context.Add(personInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personInfo);
        }

        // GET: Persons/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInfo = await _defaultPersons.SingleOrDefaultAsync(x => x.Id == id);
            if (personInfo == null)
            {
                return NotFound();
            }
            return View(personInfo);
        }

        // POST: Persons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Email,Deleted")] PersonInfo personInfo)
        {
            if (id != personInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonInfoExists(personInfo.Id))
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
            return View(personInfo);
        }

        // GET: Persons/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInfo = await _defaultPersons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personInfo == null)
            {
                return NotFound();
            }

            return View(personInfo);
        }

        // POST: Persons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var personInfo = await _defaultPersons.SingleOrDefaultAsync(x => x.Id == id);
            _context.Persons.Remove(personInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonInfoExists(Guid id)
        {
            return _defaultPersons.Any(e => e.Id == id);
        }
    }
}
