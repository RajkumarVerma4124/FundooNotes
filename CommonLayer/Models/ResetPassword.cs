using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    public class ResetPassword
    {
        //Checking The Pattern For Password And Giving Required Annotations For Password Property
        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[\W]).{8,}$", ErrorMessage = "Passsword is not valid")]
        public string NewPassword { get; set; }

        //Checking The Pattern For Password And Giving Required Annotations For Confirm Password Property
        [RegularExpression(@"(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[\W]).{8,}$", ErrorMessage = "Confirm Passsword is not valid")]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; }
    }
}
