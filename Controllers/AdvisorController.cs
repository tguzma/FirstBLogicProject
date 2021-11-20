using FirstBLogicProject.Data;
using FirstBLogicProject.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace FirstBLogicProject.Controllers
{
    public class AdvisorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdvisorController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Export()
        {
            DateTime datetime = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("First Name, Last Name, Email, Phone Number, Identification Number, Age");
            foreach (AdvisorItem advisor in _context.AdvisorItems)
            {
                sb.AppendLine($"{advisor.FirstName},{advisor.LastName},{advisor.Email},{advisor.PhoneNumber},{advisor.IdentificationNumber},{advisor.Age}");
            }


            return File(Encoding.UTF32.GetBytes(sb.ToString()), "text/csv", $"AdvisorTable_{datetime}.csv");
        }


        // GET: Advisor
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
            ViewData["AgeSortParm"] = sortOrder == "Age" ? "age_desc" : "Age";
            ViewData["CurrentFilter"] = searchString;
            var advisors = from a in _context.AdvisorItems
                           select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                advisors = advisors.Where(a => a.LastName.Contains(searchString)
                                       || a.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "age_desc":
                    advisors = advisors.OrderByDescending(a => a.Age);
                    break;
                case "Age":
                    advisors = advisors.OrderBy(a => a.Age);
                    break;
                case "email_desc":
                    advisors = advisors.OrderByDescending(a => a.Email);
                    break;
                case "Email":
                    advisors = advisors.OrderBy(a => a.Email);
                    break;
                case "name_desc":
                    advisors = advisors.OrderByDescending(a => a.LastName);
                    break;
                default:
                    advisors = advisors.OrderBy(a => a.LastName);
                    break;
            }
            return View(await advisors.AsNoTracking().ToListAsync());
        }

        // GET: Advisor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advisorItem = await _context.AdvisorItems
                .FirstOrDefaultAsync(m => m.ID == id);
            if (advisorItem == null)
            {
                return NotFound();
            }

            return View(advisorItem);
        }

        // GET: Advisor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Advisor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,Email,PhoneNumber,IdentificationNumber,Age")] AdvisorItem advisorItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(advisorItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(advisorItem);
        }

        // GET: Advisor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advisorItem = await _context.AdvisorItems.FindAsync(id);
            if (advisorItem == null)
            {
                return NotFound();
            }
            return View(advisorItem);
        }

        // POST: Advisor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,Email,PhoneNumber,IdentificationNumber,Age")] AdvisorItem advisorItem)
        {
            if (id != advisorItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advisorItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvisorItemExists(advisorItem.ID))
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
            return View(advisorItem);
        }

        // GET: Advisor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advisorItem = await _context.AdvisorItems
                .FirstOrDefaultAsync(m => m.ID == id);
            if (advisorItem == null)
            {
                return NotFound();
            }

            return View(advisorItem);
        }

        // POST: Advisor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advisorItem = await _context.AdvisorItems.FindAsync(id);
            _context.AdvisorItems.Remove(advisorItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvisorItemExists(int id)
        {
            return _context.AdvisorItems.Any(e => e.ID == id);
        }
    }

}
