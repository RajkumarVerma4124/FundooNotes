using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Class For User Notes Request
    /// </summary>
    public class UserNotes
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public IFormFile ImagePath { get; set; }
        [DefaultValue("2022-04-01 12:12:55.389Z")]
        public DateTime Reminder { get; set; }
        public bool IsArchive { get; set; }
        public bool IsPinned { get; set; }
        public bool IsTrash { get; set; }

    }
}
