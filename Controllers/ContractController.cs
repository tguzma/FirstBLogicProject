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
    public class ContractController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContractController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Export()
        {
            DateTime datetime = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Registration Number,Institution, Client, Contract Administrator, Advisors, Closing Date, Effective Date, End Date");
            foreach (ContractItem contract in _context.ContractItems)
            {
                sb.AppendLine($"{contract.RegistrationNumber},{contract.InstitutionName},{contract.ClientName},{contract.ContractAdministratorName},{contract.Advisors},{contract.ClosingDate},{contract.EffectiveDate},{contract.EndDate}");
            }

            return File(Encoding.UTF32.GetBytes(sb.ToString()), "text/csv", $"ClientTable_{datetime}.csv");
        }
        // GET: Contract
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            
            ViewData["RNSortParm"] = String.IsNullOrEmpty(sortOrder) ? "rn_desc" : "";
            ViewData["InstitutionNameSortParm"] = sortOrder == "InstitutionName" ? "in_desc" : "InstitutionName";
            ViewData["AdvisorsSortParm"] = sortOrder == "Advisors" ? "adv_desc" : "Advisors";
            ViewData["ClientNameSortParm"] = sortOrder == "ClientName" ? "cn_desc" : "ClientName";
            ViewData["ContractAdministratorNameSortParm"] = sortOrder == "ContractAdministratorName" ? "can_desc" : "ContractAdministratorName";
            ViewData["ClosingDateSortParm"] = sortOrder == "ClosingDate" ? "cd_desc" : "ClosingDate";
            ViewData["EffectiveDateSortParm"] = sortOrder == "EffectiveDate" ? "efd_desc" : "EffectiveDate";
            ViewData["EndDateSortParm"] = sortOrder == "EndDate" ? "ed_desc" : "EndDate";
            
            var contracts = from a in _context.ContractItems
                          select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                contracts = contracts.Where(a => a.RegistrationNumber.Contains(searchString)
                                       || a.InstitutionName.Contains(searchString)
                                       || a.Advisors.Contains(searchString)
                                       || a.ClientName.Contains(searchString)
                                       || a.ContractAdministratorName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "in_desc":
                    contracts = contracts.OrderByDescending(a => a.InstitutionName);
                    break;
                case "InstitutionName":
                    contracts = contracts.OrderBy(a => a.InstitutionName);
                    break;
                case "adv_desc":
                    contracts = contracts.OrderByDescending(a => a.Advisors);
                    break;
                case "Advisors":
                    contracts = contracts.OrderBy(a => a.Advisors);
                    break;
                case "cn_desc":
                    contracts = contracts.OrderByDescending(a => a.ClientName);
                    break;
                case "ClientName":
                    contracts = contracts.OrderBy(a => a.ClientName);
                    break;
                case "can_desc":
                    contracts = contracts.OrderByDescending(a => a.ContractAdministratorName);
                    break;
                case "ContractAdministratorName":
                    contracts = contracts.OrderBy(a => a.ContractAdministratorName);
                    break;
                case "cd_desc":
                    contracts = contracts.OrderByDescending(a => a.ContractAdministratorName);
                    break;
                case "ClosingDate":
                    contracts = contracts.OrderBy(a => a.ContractAdministratorName);
                    break;
                case "efd_desc":
                    contracts = contracts.OrderByDescending(a => a.ContractAdministratorName);
                    break;
                case "EffectiveDate":
                    contracts = contracts.OrderBy(a => a.ContractAdministratorName);
                    break;
                case "ed_desc":
                    contracts = contracts.OrderByDescending(a => a.ContractAdministratorName);
                    break;
                case "EndDate":
                    contracts = contracts.OrderBy(a => a.ContractAdministratorName);
                    break;
                case "rn_desc":
                    contracts = contracts.OrderByDescending(a => a.RegistrationNumber);
                    break;
                default:
                    contracts = contracts.OrderBy(a => a.RegistrationNumber);
                    break;
            }
            return View(await contracts.AsNoTracking().ToListAsync());
        }

        // GET: Contract/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractItem = await _context.ContractItems
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contractItem == null)
            {
                return NotFound();
            }

            return View(contractItem);
        }

        // GET: Contract/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,RegistrationNumber,InstitutionName,ClientName,ContractAdministratorName,Advisors,ClosingDate,EffectiveDate,EndDate")] ContractItem contractItem)
        {

            if (DateTime.Compare(contractItem.ClosingDate,contractItem.EndDate) >= 0)
            {
                ModelState.AddModelError("EndDate", "End date of contract can't be before or on closing date.");
            }
            if (DateTime.Compare(contractItem.EffectiveDate, contractItem.EndDate) >= 0)
            {
                ModelState.AddModelError("EndDate", "End date of contract can't be before or on effective date.");
            }
            if (DateTime.Compare(contractItem.ClosingDate, contractItem.EffectiveDate) > 0)
            {
                ModelState.AddModelError("EffectiveDate", "Effective date of contract can't be before closing date.");
            }

            if (ModelState.IsValid)
            {
                if (contractItem.Advisors.Contains(','))
                {
                    contractItem.ContractAdministratorName = contractItem.Advisors.Substring(0, contractItem.Advisors.IndexOf(","));
                    char[] arr = contractItem.Advisors.ToCharArray();
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if(arr[i] == ',')
                        {
                            arr[i] = ' ';
                        }
                    }
                    contractItem.Advisors = new string(arr);
                }
                else
                {
                    contractItem.ContractAdministratorName = contractItem.Advisors;
                }

                _context.Add(contractItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contractItem);
        }

        // GET: Contract/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractItem = await _context.ContractItems.FindAsync(id);
            if (contractItem == null)
            {
                return NotFound();
            }
            return View(contractItem);
        }

        // POST: Contract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,RegistrationNumber,InstitutionName,ClientName,ContractAdministratorName,Advisors,ClosingDate,EffectiveDate,EndDate")] ContractItem contractItem)
        {
            if (id != contractItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (contractItem.Advisors.Contains(','))
                    {
                        contractItem.ContractAdministratorName = contractItem.Advisors.Substring(0, contractItem.Advisors.IndexOf(","));
                        char[] arr = contractItem.Advisors.ToCharArray();
                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (arr[i] == ',')
                            {
                                arr[i] = ' ';
                            }
                        }
                        contractItem.Advisors = new string(arr);
                    }
                    else
                    {
                        contractItem.ContractAdministratorName = contractItem.Advisors;
                    }
                    _context.Update(contractItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractItemExists(contractItem.ID))
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
            return View(contractItem);
        }

        // GET: Contract/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractItem = await _context.ContractItems
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contractItem == null)
            {
                return NotFound();
            }

            return View(contractItem);
        }

        // POST: Contract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contractItem = await _context.ContractItems.FindAsync(id);
            _context.ContractItems.Remove(contractItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractItemExists(int id)
        {
            return _context.ContractItems.Any(e => e.ID == id);
        }
    }
}
