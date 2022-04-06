using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Created The Notes Business Layer Class To Implement INotesBL Methods
    /// </summary>
    public class NotesBL : INotesBL
    {
        //Reference Object For Interface IUserRL
        private readonly INotesRL notesRL;

        //Created Constructor With Dependency Injection For IUSerRL
        public NotesBL(INotesRL notesRL)
        {
            this.notesRL = notesRL;
        }

        //Method To Return Created Notes Data
        public NotesResponse CreateNote(UserNotes userNotes, long userId)
        {
            try
            {
                return notesRL.CreateNote(userNotes, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<GetNotesResponse> GetAllNotes()
        {
            try
            {
                return notesRL.GetAllNotes();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<GetNotesResponse> GetAllNotesByUserId(long userId)
        {
            try
            {
                return notesRL.GetAllNotesByUserId(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetNotesResponse GetNote(long noteId, long userId)
        {
            try
            {
                return notesRL.GetNote(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity UpdateNote(NoteUpdate noteUpdate, long noteId, long userId)
        {
            try
            {
                return notesRL.UpdateNote(noteUpdate, noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeleteNote(long noteId, long userId)
        {
            try
            {
                return notesRL.DeleteNote(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity ChangeIsArchieveStatus(long noteId, long userId)
        {
            try
            {
                return notesRL.ChangeIsArchieveStatus(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity ChangeIsPinnedStatus(long noteId, long userId)
        {
            try
            {
                return notesRL.ChangeIsPinnedStatus(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity ChangeIsTrashStatus(long noteId, long userId)
        {
            try
            {
                return notesRL.ChangeIsTrashStatus(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity ChangeColour(long noteId, long userId, string newColor)
        {
            try
            {
                return notesRL.ChangeColour(noteId, userId, newColor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<ImageEntity> AddImages(long noteId, long userId, ICollection<IFormFile> imagePath)
        {
            try
            {
                return notesRL.AddImages(noteId, userId, imagePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeleteImage(long imageId, long noteId, long userId)
        {
            try
            {
                return notesRL.DeleteImage(imageId, noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<GetNotesResponse> GetNotesByLabelName(string labelName)
        {
            try
            {
                return notesRL.GetNotesByLabelName(labelName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
