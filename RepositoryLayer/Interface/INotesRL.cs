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
        GetNotes GetNote(long noteId, long userId);
        IList<GetNotes> GetAllNotesByUserId(long userId);
        IList<GetNotes> GetAllNotes();
        NoteEntity UpdateNote(NoteUpdate noteUpdate, long noteId, long userId);
        string DeleteNote(long noteId, long userId);
        NoteEntity CheckIsArchieveOrNot(long noteId, long userId);
        NoteEntity CheckIsPinnedOrNot(long noteId, long userId);
        NoteEntity CheckIsTrashOrNot(long noteId, long userId);
    }
}
