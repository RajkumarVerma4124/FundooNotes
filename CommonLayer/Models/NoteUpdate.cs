using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Class For User Notes Update Request
    /// </summary>
    public class NoteUpdate
    {
        public string Title { get; set; }
        public string Description { get; set; }
/*        public string Color { get; set; }
        public string Image { get; set; }
        public bool IsArchive { get; set; }
        public bool IsPinned { get; set; }
        public bool IsTrash { get; set; }*/
        public DateTime Reminder { get; set; }
    }
}
