using System.ComponentModel.DataAnnotations;
namespace FirstApp.Models{

public class BookingHistory{
public Guid UserId{get;set;}
[Key]
public int BookingId{get;set;}
public string? Name{get;set;}
public string? Date{get;set;}
public string? Destination{get;set;}
    }
}