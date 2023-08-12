using FirstApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstApp.Data
{
    public class EmployeeDBContext : DbContext 
    {  
        public EmployeeDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<BookingHistory> BookingHistories { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
