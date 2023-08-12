
using FirstApp.Data;
using FirstApp.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Net.Http.Headers;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EmployeeDBContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddScoped<LogActionFilterAttribute>();
builder.Services.AddControllersWithViews();
var configuration = builder.Configuration;
var apiEndpoint = configuration["ApiEndpoint"];
            builder.Services.AddHttpClient("api", client =>
            {
                client.BaseAddress = new Uri(apiEndpoint);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("IdentityManagement/json"));
            });
builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Signup", Version = "v1" });
            });
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option=>{
option.LoginPath="/Home/Index"; 
option.ExpireTimeSpan=TimeSpan.FromMinutes(30);});

var _logger=new LoggerConfiguration().
WriteTo.File("C:\\Users\\abi6\\Desktop\\FirstApp\\Loger-.log",rollingInterval:RollingInterval.Day).CreateLogger();
 builder.Logging.AddSerilog(_logger);

builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
   
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseAuthentication();
app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Signup v1");
            });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
