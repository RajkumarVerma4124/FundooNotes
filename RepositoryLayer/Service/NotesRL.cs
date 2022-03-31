using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    /// <summary>
    /// Created The Notes Repository Layer Class To Implement INotesRL Methods
    /// </summary>
    public class NotesRL : INotesRL
    {
        //Reference Object For FundooContext
        private readonly FundooContext fundooContext;

        //Created Constructor To Initialize Fundoocontext For Each Instance
        public NotesRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        //Method To Create Note With New Notes Data And Userid Into The Db Table
        public NoteEntity CreateNote(UserNotes userNotes, long userId)
        {
            try
            {
                if (!userId.Equals(null))
                {
                    NoteEntity notesEntity = new NoteEntity();
                    notesEntity.Title = userNotes.Title;
                    notesEntity.Description = userNotes.Description;
                    notesEntity.Reminder = userNotes.Reminder;
                    notesEntity.Color = userNotes.Color;
                    notesEntity.Image = userNotes.Image;
                    notesEntity.IsTrash = userNotes.IsTrash;
                    notesEntity.IsArchive = userNotes.IsArchive;
                    notesEntity.IsPinned = userNotes.IsPinned;
                    notesEntity.CreatedAt = DateTime.Now;
                    notesEntity.ModifiedAt = DateTime.Now;
                    notesEntity.UserId = userId;
                    fundooContext.NotesData.Add(notesEntity);
                    //Adding The Data And Saving The Changes In Database
                    int res = fundooContext.SaveChanges();
                    if (res > 0)
                        return notesEntity;
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch Single Note Details By Giving Note And User Ids
        public NoteEntity GetNote(long noteId, long userId)
        {
            try
            {
                if (!userId.Equals(null))
                {
                    var resNote = fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();
                    if (resNote != null)
                        return resNote;
                    else
                        return null;
                }
                else
                    return null;                  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch Multiple Notes Details By User Ids
        public List<NoteEntity> GetAllNotesByUserId(long userId)
        {
            try
            {
                if (!userId.Equals(null))
                {
                    var resNote = fundooContext.NotesData.Where(n => n.UserId == userId).ToList();
                    if (resNote != null)
                        return resNote;
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch Multiple Notes Details From DB
        public List<NoteEntity> GetAllNotes()
        {
            try
            {
                var resNote = fundooContext.NotesData.ToList();
                if (resNote != null)
                    return resNote;
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch And Update Notes Details And SaveChanges In DB
        public NoteEntity UpdateNote(NoteUpdate noteUpdate, long noteId, long userId)
        {
            try
            {
                var resNote = fundooContext.NotesData.Where(u => u.NoteId == noteId && u.UserId == userId).FirstOrDefault();
                if (resNote != null)
                {
                    resNote.Title = string.IsNullOrEmpty(noteUpdate.Title) ? resNote.Title : noteUpdate.Title;
                    resNote.Description = string.IsNullOrEmpty(noteUpdate.Description) ? resNote.Description : noteUpdate.Description;
                    resNote.Reminder = noteUpdate.Reminder.CompareTo(resNote.Reminder) == 0 ? resNote.Reminder : noteUpdate.Reminder;
                    resNote.Color = string.IsNullOrEmpty(noteUpdate.Color) ? resNote.Color : noteUpdate.Color;
                    resNote.Image = string.IsNullOrEmpty(noteUpdate.Image) ? resNote.Image : noteUpdate.Image;
                    resNote.IsTrash = noteUpdate.IsTrash.CompareTo(resNote.IsTrash) == 0 ? resNote.IsTrash : noteUpdate.IsTrash;
                    resNote.IsArchive = noteUpdate.IsArchive.CompareTo(resNote.IsArchive) == 0 ? resNote.IsArchive : noteUpdate.IsArchive;
                    resNote.IsPinned = noteUpdate.IsPinned.CompareTo(resNote.IsPinned) == 0 ? resNote.IsPinned : noteUpdate.IsPinned;
                    resNote.ModifiedAt = DateTime.Now;

                    // Updating And Saving The Changes In The Database For Given NoteId.
                    fundooContext.NotesData.Update(resNote);
                    fundooContext.SaveChanges();
                    return resNote;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch And Delete Note Using Note Id
        public string DeleteNote(long noteId, long userId)
        {
            try
            {
                var resNote = fundooContext.NotesData.Where(u => u.NoteId == noteId && u.UserId == userId).FirstOrDefault();
                if (resNote != null)
                {
                    // Deleting And Saving The Changes In The Database For Given NoteId.
                    fundooContext.NotesData.Remove(resNote);
                    fundooContext.SaveChanges();
                    return "Deleted The Note Successfully";
                }
                else
                    return "Note Deletion Failed";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
