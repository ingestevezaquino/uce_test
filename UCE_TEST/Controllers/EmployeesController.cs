using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using UCE_TEST;
using UCE_TEST.Models.DTOs;
using UCE_TEST.Models;
using Newtonsoft.Json;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCE_TEST.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string currentFilter, string searchString, int? page)
        {
            IEnumerable<Employee> employees = await _context.Employees.Where(e => e.IsActive.Equals(true)).Include(e => e.Address).ToListAsync();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.IsActive.Equals(true)).Where(e => e.Name.ToUpper().Contains(searchString.ToUpper())
                                       || e.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || e.DateOfHire.ToString("dd/MM/yyyy").Contains(searchString))
                                       .ToList();
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return _context.Employees.Where(e => e.IsActive.Equals(true)).Include(e => e.Address) != null ? 
                          View(await employees.ToPagedListAsync(pageNumber, pageSize)) :
                          Problem("Entity set 'ApplicationDbContext.Employees' is null.");
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.Include(m => m.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Provinces = await GetProvincesDO();
            ViewBag.MaritalStatus = new SelectList(Enum.GetValues<Models.Enums.MaritalState>());
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LastName,Position,BirthDay,DateOfHire,Phone,Email,MaritalStatus,Address")] Employee employee, IFormFile photo)
        {
            if (photo is not null)
            {
                try
                {
                    using var memoryStream = new MemoryStream();
                    photo.OpenReadStream().CopyTo(memoryStream);
                    employee.Photo = memoryStream.ToArray();
                }
                catch
                {
                    return Problem("UNABLE TO PROCESS UPLOADED PHOTO");
                }
            }

            if (employee.Address is not null && ModelState["Address.Employee"].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                ModelState["Address.Employee"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Provinces = await GetProvincesDO();
            ViewBag.MaritalStatus = new SelectList(Enum.GetValues<Models.Enums.MaritalState>());
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Where(e => e.IsActive.Equals(true))
                .Include(e => e.Address)
                .FirstOrDefaultAsync(e => e.Id.Equals(id));

            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.Provinces = await GetProvincesDO();
            ViewBag.MaritalStatus = new SelectList(Enum.GetValues<Models.Enums.MaritalState>());
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName,Position,BirthDay,DateOfHire,Phone,Email,MaritalStatus,Address")] Employee employee, IFormFile photo)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            var employeeFromDB = await _context.Employees.AsNoTracking()
                        .Where(e => e.IsActive.Equals(true))
                        .Include(e => e.Address)
                        .FirstOrDefaultAsync(e => e.Id.Equals(employee.Id));

            employee.IsActive = employeeFromDB.IsActive;
            employee.Address.Id = employeeFromDB.Address.Id;

            if (photo is null)
            {
                employee.Photo = employeeFromDB.Photo;
                ModelState["photo"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            }
            else
            {
                try
                {
                    using var memoryStream = new MemoryStream();
                    photo.OpenReadStream().CopyTo(memoryStream);
                    employee.Photo = memoryStream.ToArray();
                }
                catch
                {
                    return Problem("UNABLE TO PROCESS UPLOADED PHOTO");
                }
            }

            if (employee.Address is not null && ModelState["Address.Employee"].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                ModelState["Address.Employee"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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

            ViewBag.Provinces = await GetProvincesDO();
            ViewBag.MaritalStatus = new SelectList(Enum.GetValues<Models.Enums.MaritalState>());
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Where(m => m.IsActive.Equals(true))
                .Include(m => m.Address)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.IsActive = false;
                _context.Employees.Update(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("api/[controller]/modifyContactMethods")]
        public async Task<IActionResult> ModifyContactMethods(int id, string phone, string email)
        {
            if (id == 0)
                return BadRequest("The 'id' Query Param must be provided");

            var employee = await _context.Employees.Where(m => m.Id.Equals(id)).FirstOrDefaultAsync();

            if (employee is null)
                return NotFound($"There is not an employee that holds the id: {id}");

            var log = new Log()
            {
                Description = $"La información de contacto del empleado con el Id: '{id}', " +
                $"fue modificada de la siguiente manera: ",
                UpdatedAt = DateTime.Now
            };

            log.Description += !String.IsNullOrEmpty(phone) && !phone.Equals(employee.Phone) ? $" ---> [Telefono Anterior] = {employee.Phone} & " +
                $"[Telefono Actual] = {phone} |" : "";

            log.Description += !String.IsNullOrEmpty(email) && !email.Equals(employee.Email) ? $" ---> [Correo Anterior] = {employee.Email} & " +
                $"[Correo Actual] = {email} |" : "";

            if (!log.Description.EndsWith("|"))
            {
                return Ok();
            }

            employee.Phone = !String.IsNullOrEmpty(phone) ? phone : employee.Phone;
            employee.Email = !String.IsNullOrEmpty(email) ? email : employee.Email;

            _context.Employees.Update(employee);
            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool EmployeeExists(int id)
        {
          return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<SelectList> GetProvincesDO()
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

            var response = await client.GetStringAsync("http://provinciasrd.raydelto.org/provincias");
            ProvinceResponse provinces = JsonConvert.DeserializeObject<ProvinceResponse>(response);

            return new SelectList(provinces.Data.Select(x => x.Nombre));
        }
    }
}
