using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net;
using System.Configuration;
using System.Net.Mail;
using System.Data.SqlClient; 
public class AccountController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration configuration;
    public AccountController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5225/");
        this.configuration=configuration;
    }

    [HttpGet]
public IActionResult Signup()
{
    var viewModel = new Register();
    return View("Signup");
}



   [HttpPost]
public async Task<IActionResult> Signup(Register Register)
{
    if (!ModelState.IsValid)
    {
        var newRegister = new Register
        {   EmployeeId = Guid.NewGuid(), 
            Name = Register.Name,
            EmailId = Register.EmailId,
            userPassword = Register.userPassword,
            DateOfBirth = Register.DateOfBirth,
            Department = Register.Department,
        };
        return View(newRegister);
    }

    var signupResponse = await _httpClient.PostAsJsonAsync("/Signup", Register);

    if (signupResponse.IsSuccessStatusCode)
    {
        // Signup was successful, redirect to the login page
        return RedirectToAction("Index", "Admin");
    }
    else
    {
        // Signup failed, display an error message to the user
        var errorMessage = await signupResponse.Content.ReadAsStringAsync();
        ModelState.AddModelError("", errorMessage);
        return View(Register);
    }
}


    }