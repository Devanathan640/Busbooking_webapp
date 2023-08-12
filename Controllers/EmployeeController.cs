
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using FirstApp.Models;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace FirstApp.Controllers
{
    public class EmployeeController:Controller{
        public  static string? Mail{get;set;}
        public  static string? password{get;set;} 
         private readonly ILogger<HomeController> _logger;
    public EmployeeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [HttpGet]  
    public IActionResult login()
    {
        ClaimsPrincipal claimUser=HttpContext.User;
        if(claimUser.Identity.IsAuthenticated){

            return RedirectToAction("Index","Dashboard");
        }
        TempData["pass"]=password;
            return View();
    }
   
    [HttpPost]
     public async Task<IActionResult> login(Employee employee,Database database)
    {
    try{
       //connection connect=new connection();
       string result= Database.login(employee);
       
       if(result=="success")
       {
        List<Claim> claims=new List<Claim>(){
            new Claim(ClaimTypes.NameIdentifier,employee.EmailId)
        };
        ClaimsIdentity claimsIdentity=new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
        AuthenticationProperties properties=new AuthenticationProperties(){
            AllowRefresh=true,
            IsPersistent=database.KeepLoggedIn
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
        new ClaimsPrincipal(claimsIdentity),properties);
        _logger.LogInformation(employee.EmailId+"Employee Login Triggered");
        TempData["mail"]=employee.EmailId;
       // Log.Information("Employee Login Triggered");  
        return RedirectToAction("Index","Dashboard",employee);
       }
       else if (result=="Admin")
       {
        List<Claim> claims=new List<Claim>(){
            new Claim(ClaimTypes.NameIdentifier,employee.EmailId)
        };
        ClaimsIdentity claimsIdentity=new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
        AuthenticationProperties properties=new AuthenticationProperties(){
            AllowRefresh=true,
            IsPersistent=database.KeepLoggedIn
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
        new ClaimsPrincipal(claimsIdentity),properties);
        return RedirectToAction("Index","Admin");
       }
    
    }
    catch(Exception ex){
        Console.WriteLine("Error raised when employee is logged in !"+ex.Message);
        TempData["msg"]="An error occured please try agin later";
    }
    
       Log.Information("Employee Login Triggered");
       ViewBag.Message="Login fails,Try Again!";
       return View("Login",employee); 
    }
    [HttpGet]
    public IActionResult forgotPassword()
    {
         return View();
    }
    [HttpPost]
    public IActionResult forgotPassword(Employee employee)
    {   
      string mail=Database.sendEmail(employee.EmailId,employee);
      Mail=employee.EmailId;
        if (mail=="sent")
        {
            ViewBag.OTP="Verification Code sent successfuly \n Enter otp to continue";
            return RedirectToAction("checkCode","Employee",employee);
        }
        else
        {
            ViewBag.Message="Your are not a registered user";
            return View("forgotPassword");
        }
    }

    [HttpGet]
    public IActionResult checkCode()
    {
         return View();
    }

    public IActionResult checkCode(Database database)
    {  
      string code=Database.verifyCode(database);
        if (code=="code sent")
        { 
            password=Database.sendPassword(Mail);
            return RedirectToAction("login","Employee");
        }
        else
        {
            ViewBag.OTP="Incorrect Code";
            return View("checkCode",database);
        }
    }
}
}
