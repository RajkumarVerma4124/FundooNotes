using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
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
        IEnumerable<GetNotes> GetAllNotesByUserId(long userId);
        IEnumerable<GetNotes> GetAllNotes();
        NoteEntity UpdateNote(NoteUpdate noteUpdate, long noteId, long userId);
        string DeleteNote(long noteId, long userId);
        NoteEntity ChangeIsArchieveStatus(long noteId, long userId);
        NoteEntity ChangeIsPinnedStatus(long noteId, long userId);
        NoteEntity ChangeIsTrashStatus(long noteId, long userId);
        NoteEntity ChangeColour(long noteId, long userId, string newColor);
        NoteEntity UpdateImage(long noteId, long userId, IFormFile imageFile);
        string DeleteImage(long noteId, long userId);
    }
}
