using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProblems.ViewModels
{
    public class SignUpViewModel
    {
        [Required]
        [Display(Name="Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Passowrd")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password",ErrorMessage = "Passwords don't match")]
        [Display(Name = "PasswordConfirm")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
