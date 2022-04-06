using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Class For Notes Label Request
    /// </summary>
    public class NotesLabel
    {
        [Required(ErrorMessage = "{0} should not be empty")]
        public long NoteId { get; set; }

        //Checking The Pattern For Email And Giving Required Annotations For EmailId property
        [Required(ErrorMessage = "{0} should not be empty")]
        public string LabelName { get; set; }
    }
}
