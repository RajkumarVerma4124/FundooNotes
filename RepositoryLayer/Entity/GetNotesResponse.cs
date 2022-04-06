using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Class For User Get Notes Response
    /// </summary>
    public class GetNotesResponse
    {
        public long NotesId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public IEnumerable<ImageEntity> ImageList { get; set; }
        public object Reminder { get; set; }
        public bool IsArchive { get; set; }
        public bool IsPinned { get; set; }
        public bool IsTrash { get; set; }
        public object CreatedAt { get; set; }
        public object ModifiedAt { get; set; }
    }
}