using FirstApp.Data;
using FirstApp.Models;
using FirstApp.Filters;
using FirstApp.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FirstApp.Controllers
{ 
    public class AdminController : Controller
    {    
        private readonly EmployeeDBContext employeeDBContext;

        public AdminController(EmployeeDBContext employeeDBContext)
        {
            this.employeeDBContext = employeeDBContext;
            }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var empolyees = await employeeDBContext.Employees.ToListAsync();
            
            return View(empolyees);
        }
      
      
       
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
      
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee()
            {
                EmployeeId = Guid.NewGuid(), 
                Name = addEmployeeRequest.Name,
                EmailId = addEmployeeRequest.Email,
                userPassword=addEmployeeRequest.userPassword,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth,

                
            };

            await employeeDBContext.Employees.AddAsync(employee);
            await employeeDBContext.SaveChangesAsync();
            return RedirectToAction("Index");
            }
              return View();
            
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await employeeDBContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);

            if(employee != null)
            {
                
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.EmployeeId, 
                    Name = employee.Name,
                    Email = employee.EmailId,
                    userPassword = employee.userPassword,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth,
                };
                return await Task.Run(() => View("View", viewModel));
            }

            return View(employee); 
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        { 
            if (ModelState.IsValid)
            {
                var employee = await employeeDBContext.Employees.FindAsync(model.Id);

            if (employee != null)
            { 
                employee.Name = model.Name;
                employee.EmailId = model.Email;
                employee.userPassword = model.userPassword;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                await employeeDBContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
                
            }
            return View();
            
        }
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await employeeDBContext.Employees.FindAsync(model.Id);  

            if (employee != null)
            {
                employeeDBContext.Employees.Remove(employee);

                await employeeDBContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
           return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> showHistory()
        {
            var empolyees = await employeeDBContext.BookingHistories.ToListAsync();
            return View(empolyees);
        }
        
    }
} 
