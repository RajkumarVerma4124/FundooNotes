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
        /// <summary>
        /// Reference Object For Interface IUserRL
        /// </summary>
        private readonly INotesRL notesRL;

        /// <summary>
        /// Created Constructor With Dependency Injection For IUSerRL
        /// </summary>
        /// <param name="notesRL"></param>
        public NotesBL(INotesRL notesRL)
        {
            this.notesRL = notesRL;
        }

        /// <summary>
        /// Method To Return Created Notes Data Method
        /// </summary>
        /// <param name="userNotes"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer GetAllNotes Method
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer GetAllNotesByUserId Method
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer GetNote Method
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer UpdateNote Method
        /// </summary>
        /// <param name="noteUpdate"></param>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer DeleteNote Method
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer ChangeIsArchieveStatus Method
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer ChangeIsPinnedStatus Method
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer ChangeIsTrashStatus Method
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer ChangeColour Method
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <param name="newColor"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer AddImages Method
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <param name="imagePath"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer DeleteImage Method
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer GetNotesByLabelId Method
        /// </summary>
        /// <param name="labelId"></param>
        /// <returns></returns>
        public IEnumerable<GetNotesResponse> GetNotesByLabelId(long labelId)
        {
            try
            {
                return notesRL.GetNotesByLabelId(labelId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
