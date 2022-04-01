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
                //Adding The Data And Saving The Changes In Database
                fundooContext.NotesData.Add(notesEntity);
                int res = fundooContext.SaveChanges();
                if (res > 0)
                    return notesEntity;
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch Single Note Details By Giving Note And User Ids
        public GetNotes GetNote(long noteId, long userId)
        {
            try
            {
                var resultNote = fundooContext.NotesData.Where(n => n.UserId == userId && n.NoteId == noteId).FirstOrDefault();
                if (resultNote != null)
                {
                    GetNotes resNote = new GetNotes();
                    resNote.NotesId = resultNote.NoteId;
                    resNote.Title = resultNote.Title;
                    resNote.Description = resultNote.Description;
                    resNote.Color = resultNote.Color;
                    resNote.Image = resultNote.Image;
                    resNote.IsArchive = resultNote.IsArchive;
                    resNote.IsTrash = resultNote.IsArchive;
                    resNote.IsPinned = resultNote.IsPinned;
                    resNote.Description = resultNote.Description;
                    resNote.Reminder = resultNote.Reminder.ToString("dd-MM-yyyy hh:mm:ss tt");
                    resNote.CreatedAt = resultNote.CreatedAt.ToString("dd-MM-yyyy hh:mm:ss tt");
                    resNote.ModifiedAt = resultNote.ModifiedAt.ToString("dd-MM-yyyy hh:mm:ss tt");
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

        //Method To Fetch Multiple Notes Details By User Ids
        public IEnumerable<GetNotes> GetAllNotesByUserId(long userId)
        {
            try
            {
                IList<GetNotes> noteList = new List<GetNotes>();
                var resNotesList = fundooContext.NotesData.Where(n => n.UserId == userId).ToList();
                if (resNotesList.Count() > 0) 
                { 
                    foreach (var notes in resNotesList)
                    {
                        GetNotes resNote = new GetNotes();
                        resNote.NotesId = notes.NoteId;
                        resNote.Title = notes.Title;
                        resNote.Description = notes.Description;
                        resNote.Color = notes.Color;
                        resNote.Image = notes.Image;
                        resNote.IsArchive = notes.IsArchive;
                        resNote.IsTrash = notes.IsArchive;
                        resNote.IsPinned = notes.IsPinned;
                        resNote.Description = notes.Description;
                        resNote.Reminder = notes.Reminder.ToString("dd-MM-yyyy hh:mm:ss tt");
                        resNote.CreatedAt = notes.CreatedAt.ToString("dd-MM-yyyy hh:mm:ss tt");
                        resNote.ModifiedAt = notes.ModifiedAt.ToString("dd-MM-yyyy hh:mm:ss tt");
                        noteList.Add(resNote);
                    }
                    return noteList; 
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
        public IEnumerable<GetNotes> GetAllNotes()
        {
            try
            {
                IList<GetNotes> noteList = new List<GetNotes>();
                var resNotesList = fundooContext.NotesData.ToList();
                if (resNotesList.Count() > 0)
                {
                    foreach (var notes in resNotesList)
                    {
                        GetNotes resNote = new GetNotes();
                        resNote.NotesId = notes.NoteId;
                        resNote.Title = notes.Title;
                        resNote.Description = notes.Description;
                        resNote.Color = notes.Color;
                        resNote.Image = notes.Image;
                        resNote.IsArchive = notes.IsArchive;
                        resNote.IsTrash = notes.IsArchive;
                        resNote.IsPinned = notes.IsPinned;
                        resNote.Description = notes.Description;
                        resNote.Reminder = notes.Reminder.ToString("dd-MM-yyyy hh:mm:ss tt");
                        resNote.CreatedAt = notes.CreatedAt.ToString("dd-MM-yyyy hh:mm:ss tt");
                        resNote.ModifiedAt = notes.ModifiedAt.ToString("dd-MM-yyyy hh:mm:ss tt");
                        noteList.Add(resNote);
                    }
                    return noteList;
                }
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
                var resNote = fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();
                if (resNote != null)
                {
                    resNote.Title = string.IsNullOrEmpty(noteUpdate.Title) ?  resNote.Title : noteUpdate.Title;
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
                var resNote = fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();
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

        //Method To Fetch The Notes Details And Turn IsArchive If True Then False And Vice Versa
        public NoteEntity ChangeIsArchieveStatus(long noteId, long userId)
        {
            try
            {
                var resNote = fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();
                if (resNote != null)
                {
                    if (resNote.IsArchive == false)
                        resNote.IsArchive = true;
                    else
                        resNote.IsArchive = false;
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

        //Method To Fetch The Notes Details And Turn IsPinned If True Then False And Vice Versa
        public NoteEntity ChangeIsPinnedStatus(long noteId, long userId)
        {
            try
            {
                var resNote = fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();  
                if (resNote != null)
                {
                    if (resNote.IsPinned == false)
                        resNote.IsPinned = true;
                    else
                        resNote.IsPinned = false;
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

        //Method To Fetch The Notes Details And Turn IsTrash If True Then False And Vice Versa
        public NoteEntity ChangeIsTrashStatus(long noteId, long userId)
        {
            try
            {
                var resNote = fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();
                if (resNote != null)
                {
                    if (resNote.IsTrash == false)
                        resNote.IsTrash = true;
                    else
                        resNote.IsTrash = false;
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
    }
}
