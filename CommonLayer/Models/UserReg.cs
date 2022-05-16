using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Class For User Reg Request
    /// </summary>
    public class UserReg
    {
        //Checking The Pattern For First Name And Giving Required Annotations For First Name Property
        [Required(ErrorMessage = "{0} should not be empty")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "First name should starts with Cap and should have minimum 3 characters")]
        [RegularExpression(@"^[A-Z]{1}[a-z]{2,}$", ErrorMessage = "First name is not valid")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        //Checking The Pattern For Last Name And Giving Required Annotations For Last Name Property
        [Required(ErrorMessage = "{0} should not be empty")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Lirst name should starts with Cap and should have minimum 3 characters")]
        [RegularExpression(@"^[A-Z]{1}[a-z]{2,}$", ErrorMessage = "Last name is not valid")]
        [DataType(DataType.Text)] public string LastName { get; set; }

        //Checking The Pattern For Email And Giving Required Annotations For EmailId property
        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"^[a-zA-Z0-9]{3,}([._+-][0-9a-zA-Z]{2,})*@[0-9a-zA-Z]+[.]?([a-zA-Z]{2})+[.]([a-zA-Z]{3})+$", ErrorMessage = "Email id is not valid")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        //Checking The Pattern For Password And Giving Required Annotations For Password Property
        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[\W]).{8,}$", ErrorMessage = "Passsword is not valid")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
