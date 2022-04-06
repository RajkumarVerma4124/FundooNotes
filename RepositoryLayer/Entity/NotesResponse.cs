using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Entity
{
    /// <summary>
    /// Created The Class For CreateNotesResponse
    /// </summary>
    public class NotesResponse
    {
        public NoteEntity NotesDetails { get; set; }
        public IEnumerable<ImageEntity> ImagesDetails { get; set; }
    }
}