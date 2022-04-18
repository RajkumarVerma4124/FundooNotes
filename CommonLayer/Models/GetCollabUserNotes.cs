using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Creating The Model class For To Get Collaboarated Users Notes Data
    /// </summary>
    public class GetCollabUserNotes
    {
        [DefaultValue("false")]
        public bool IsPinned { get; set; }
        [DefaultValue("false")]
        public bool IsArchive { get; set; }
        public string NoteColor { get; set; }
    }
}
