using System.ComponentModel.DataAnnotations;

namespace FirstApp.Models.Admin
{
   
    public class UpdateEmployeeViewModel
    {
        public Guid Id { get; set; }
        
        [StringLength(20,MinimumLength =3,ErrorMessage ="Please Enter a Valid Name")]
        public string Name { get; set; }
        public string Email { get; set; }
        public string userPassword { get; set; }
       
        public DateTime DateOfBirth { get; set; }
        public string Department { get; set; }
    }
}
