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
        NotesResponse CreateNote(UserNotes userNotes, long userId);
        GetNotesResponse GetNote(long noteId, long userId);
        IEnumerable<GetNotesResponse> GetAllNotesByUserId(long userId);
        IEnumerable<GetNotesResponse> GetAllNotes();
        IEnumerable<GetNotesResponse> GetNotesByLabelName(string labelName);
        NoteEntity UpdateNote(NoteUpdate noteUpdate, long noteId, long userId);
        string DeleteNote(long noteId, long userId);
        NoteEntity ChangeIsArchieveStatus(long noteId, long userId);
        NoteEntity ChangeIsPinnedStatus(long noteId, long userId);
        NoteEntity ChangeIsTrashStatus(long noteId, long userId);
        NoteEntity ChangeColour(long noteId, long userId, string newColor);
        IEnumerable<ImageEntity> AddImages(long noteId, long userId, ICollection<IFormFile> imagePath);
        string DeleteImage(long imageId, long noteId, long userId);
    }
}
