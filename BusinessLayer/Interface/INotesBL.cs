using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
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
        GetNotes GetNote(long noteId, long userId);
        IEnumerable<GetNotes> GetAllNotesByUserId(long userId);
        IEnumerable<GetNotes> GetAllNotes();
        NoteEntity UpdateNote(NoteUpdate noteUpdate, long noteId, long userId);
        string DeleteNote(long noteId, long userId);
        NoteEntity ChangeIsArchieveStatus(long noteId, long userId);
        NoteEntity ChangeIsPinnedStatus(long noteId, long userId);
        NoteEntity ChangeIsTrashStatus(long noteId, long userId);
        NoteEntity ChangeColour(long noteId, long userId, string newColor);
        NoteEntity UpdateImage(long noteId, long userId, IFormFile imagePath);
        string DeleteImage(long noteId, long userId);
    }
}
