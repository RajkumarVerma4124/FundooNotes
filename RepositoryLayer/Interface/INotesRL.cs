using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    /// <summary>
    /// Created The User Interface For Notes Repository Layer Class
    /// </summary>
    public interface INotesRL
    {
        NoteEntity CreateNote(UserNotes userNotes, long userId);
        NoteEntity GetNote(long noteId, long userId);
        List<NoteEntity> GetAllNotesByUserId(long userId);
        List<NoteEntity> GetAllNotes();
        NoteEntity UpdateNote(NoteUpdate noteUpdate, long noteId, long userId);
        string DeleteNote(long noteId, long userId);
    }
}
