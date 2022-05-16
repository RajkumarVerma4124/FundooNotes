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
        GetNotesResponse CreateNote(UserNotes userNotes, long userId);
        GetNotesResponse GetNote(long noteId, long userId);
        IEnumerable<GetNotesResponse> GetAllNotesByUserId(long userId);
        IEnumerable<GetNotesResponse> GetAllNotes();
        IEnumerable<GetNotesResponse> GetNotesByLabelId(long labelId);
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
