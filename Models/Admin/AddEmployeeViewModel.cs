using System.ComponentModel.DataAnnotations;

namespace FirstApp.Models.Admin
{
    public class AddEmployeeViewModel
    {
        [StringLength(20,MinimumLength =3,ErrorMessage ="Please Enter a Valid Name")]
        public string Name { get; set; }
         [Required(ErrorMessage = "Enter e-mail")]
         [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string userPassword { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        public string Department { get; set; }

    }
}
