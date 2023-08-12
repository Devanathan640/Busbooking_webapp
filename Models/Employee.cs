
using System.ComponentModel.DataAnnotations;

namespace FirstApp.Models;
public class Employee{
  public Guid EmployeeId{get;set;}
  public string? Name{get;set;}

  [Required(ErrorMessage = "Enter e-mail")]
  [EmailAddress]
  public string? EmailId{get;set;}

  [Required(ErrorMessage = "Enter Password")]
  [DataType(DataType.Password)]
  
  public string? userPassword{get;set;}
   public DateTime DateOfBirth { get; set; }

    public string? Department{get;set;}
}
