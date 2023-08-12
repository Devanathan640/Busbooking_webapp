
using Microsoft.AspNetCore.Mvc;
using FirstApp.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using FirstApp.Data;
using Microsoft.EntityFrameworkCore;

namespace FirstApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
{

    private readonly EmployeeDBContext employeeDBContext;

        public DashboardController(EmployeeDBContext employeeDBContext)
        {
            this.employeeDBContext = employeeDBContext;
            }
        [HttpGet]
        public async Task<IActionResult> displayHistory()
        {
            var empolyees = await employeeDBContext.BookingHistories.Where(b=>b.UserId==CurrentUser.UserId).ToListAsync();
            return View(empolyees);
        }



    public IActionResult Index(Employee employee)
    {
        try
        {
        HttpContext.Session.SetString("UserName",CurrentUser.display(employee.EmailId));
        return View();
        }
        catch (Exception)
        {    
            return View();
        }
    }

    public ActionResult showProfile(){
        return View();
    }
    [HttpGet]
    public ActionResult bookRide(){
        return View();
    }
    [HttpPost]
    public ActionResult bookRide(CurrentUser currentUser,Employee employee){
      
    string? status=CurrentUser.bookBus(currentUser.Destination,currentUser.Date);
        if(status=="Success"){
            // string notification=CurrentUser.sendNotification(employee.EmailId,employee,currentUser);
             
             return RedirectToAction("seatBooked",currentUser);
            
        }
        else if(status=="Leave")
        {
            ViewBag.message="Please Select  Working Days !!";
            return View();
        }
            ViewBag.message="Please Select Upcomming Days !!";
            return View();
           
        
    }
    // [HttpGet]
    public ActionResult seatBooked(){
        return View();
    }


    public ActionResult contact(){
        return View();
    }
    
        
}
}