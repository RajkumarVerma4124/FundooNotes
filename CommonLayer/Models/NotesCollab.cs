using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Class For User Reg Request
    /// </summary>
    public class NotesCollab
    {
        public long NoteId { get; set; }
        public long UserId { get; set; }
        //Checking The Pattern For Email And Giving Required Annotations For EmailId property
        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"^[a-zA-Z0-9]{3,}([._+-][0-9a-zA-Z]{2,})*@[0-9a-zA-Z]+[.]?([a-zA-Z]{2,4})+[.]?([a-zA-Z]{2,3})*$", ErrorMessage = "Email id is not valid")]
        [DataType(DataType.EmailAddress)]
        public string CollabEmail { get; set; }
    }
}
