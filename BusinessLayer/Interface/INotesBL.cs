using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Created The Interface For Notes Business Layer Class 
    /// </summary>
    public interface INotesBL
    {
        NoteEntity CreateNote(UserNotes userNotes, long userId);
        NoteEntity GetNote(long noteId, long userId);
        List<NoteEntity> GetAllNotesByUserId(long userId);
        List<NoteEntity> GetAllNotes();
        NoteEntity UpdateNote(NoteUpdate noteUpdate, long noteId, long userId);
        string DeleteNote(long noteId, long userId);
        NoteEntity CheckIsArchieveOrNot(long noteId, long userId);
        NoteEntity CheckIsPinnedOrNot(long noteId, long userId);
        NoteEntity CheckIsTrashOrNot(long noteId, long userId);
    }
}
