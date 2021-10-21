using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProblems.ViewModels
{
    public class SignInViewModel
    {
        [Required]
        [Display(Name="Name")]
        public string Name { get; set; }     
        [Required]
        [Display(Name = "Passowrd")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
