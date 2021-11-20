using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstBLogicProject.Data;
using FirstBLogicProject.Models.Entity;
using System.Text;

namespace FirstBLogicProject.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
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
            foreach (ClientItem client in _context.ClientItems)
            {
                sb.AppendLine($"{client.FirstName},{client.LastName},{client.Email},{client.PhoneNumber},{client.IdentificationNumber},{client.Age}");
            }

            return File(Encoding.UTF32.GetBytes(sb.ToString()), "text/csv", $"ClientTable_{datetime}.csv");
        }

        // GET: Client
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
            ViewData["AgeSortParm"] = sortOrder == "Age" ? "age_desc" : "Age";
            ViewData["CurrentFilter"] = searchString;
            var clients = from a in _context.ClientItems
                           select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(a => a.LastName.Contains(searchString)
                                       || a.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "age_desc":
                    clients = clients.OrderByDescending(a => a.Age);
                    break;
                case "Age":
                    clients = clients.OrderBy(a => a.Age);
                    break;
                case "email_desc":
                    clients = clients.OrderByDescending(a => a.Email);
                    break;
                case "Email":
                    clients = clients.OrderBy(a => a.Email);
                    break;
                case "name_desc":
                    clients = clients.OrderByDescending(a => a.LastName);
                    break;
                default:
                    clients = clients.OrderBy(a => a.LastName);
                    break;
            }
            return View(await clients.AsNoTracking().ToListAsync());
        }

        // GET: Client/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientItem = await _context.ClientItems
                .FirstOrDefaultAsync(m => m.ID == id);
            if (clientItem == null)
            {
                return NotFound();
            }

            return View(clientItem);
        }

        // GET: Client/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,Email,PhoneNumber,IdentificationNumber,Age")] ClientItem clientItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientItem);
        }

        // GET: Client/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientItem = await _context.ClientItems.FindAsync(id);
            if (clientItem == null)
            {
                return NotFound();
            }
            return View(clientItem);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,Email,PhoneNumber,IdentificationNumber,Age")] ClientItem clientItem)
        {
            if (id != clientItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientItemExists(clientItem.ID))
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
            return View(clientItem);
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientItem = await _context.ClientItems
                .FirstOrDefaultAsync(m => m.ID == id);
            if (clientItem == null)
            {
                return NotFound();
            }

            return View(clientItem);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientItem = await _context.ClientItems.FindAsync(id);
            _context.ClientItems.Remove(clientItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientItemExists(int id)
        {
            return _context.ClientItems.Any(e => e.ID == id);
        }
    }
}
