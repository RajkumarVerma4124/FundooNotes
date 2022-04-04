using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryLayer.Service
{
    /// <summary>
    /// Created The Notes Repository Layer Class To Implement INotesRL Methods
    /// </summary>
    public class NotesRL : INotesRL
    {
        //Reference Object For FundooContext
        private readonly FundooContext fundooContext;

        //Reference Object For configuration
        private readonly IConfiguration configuration;


        //Created Constructor To Initialize Fundoocontext For Each Instance
        public NotesRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
        }

        //Method To Create Note With New Notes Data And Userid Into The Db Table
        public NoteEntity CreateNote(UserNotes userNotes, long userId)
        {
            try
            {
                NoteEntity notesEntity = new NoteEntity
                {
                    Title = userNotes.Title,
                    Description = userNotes.Description,
                    Reminder = userNotes.Reminder,
                    Color = userNotes.Color,
                    Image = UploadImage(userNotes.ImagePath).Url.ToString(),
                    IsTrash = userNotes.IsTrash,
                    IsArchive = userNotes.IsArchive,
                    IsPinned = userNotes.IsPinned,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    UserId = userId
                };
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
                    GetNotes resNote = new GetNotes
                    {
                        NotesId = resultNote.NoteId,
                        Title = resultNote.Title,
                        Description = resultNote.Description,
                        Color = resultNote.Color,
                        Image = resultNote.Image,
                        IsArchive = resultNote.IsArchive,
                        IsTrash = resultNote.IsArchive,
                        IsPinned = resultNote.IsPinned,
                        Reminder = resultNote.Reminder.ToString("dd-MM-yyyy hh:mm:ss tt"),
                        CreatedAt = resultNote.CreatedAt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                        ModifiedAt = resultNote.ModifiedAt.ToString("dd-MM-yyyy hh:mm:ss tt")
                    };

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
                    foreach (var resultNote in resNotesList)
                    {
                        GetNotes note = new GetNotes
                        {
                            NotesId = resultNote.NoteId,
                            Title = resultNote.Title,
                            Description = resultNote.Description,
                            Color = resultNote.Color,
                            Image = resultNote.Image,
                            IsArchive = resultNote.IsArchive,
                            IsTrash = resultNote.IsArchive,
                            IsPinned = resultNote.IsPinned,
                            Reminder = resultNote.Reminder.ToString("dd-MM-yyyy hh:mm:ss tt"),
                            CreatedAt = resultNote.CreatedAt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                            ModifiedAt = resultNote.ModifiedAt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                        };
                        noteList.Add(note);
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
                    foreach (var resultNote in resNotesList)
                    {
                        GetNotes note = new GetNotes
                        {
                            NotesId = resultNote.NoteId,
                            Title = resultNote.Title,
                            Description = resultNote.Description,
                            Color = resultNote.Color,
                            Image = resultNote.Image,
                            IsArchive = resultNote.IsArchive,
                            IsTrash = resultNote.IsArchive,
                            IsPinned = resultNote.IsPinned,
                            Reminder = resultNote.Reminder.ToString("dd-MM-yyyy hh:mm:ss tt"),
                            CreatedAt = resultNote.CreatedAt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                            ModifiedAt = resultNote.ModifiedAt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                        };
                        noteList.Add(note);
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
                    return "Note With Given Id Not Found For Deletion";
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

        //Method To Fetch The Notes Details And Changed The Color Of The Note
        public NoteEntity ChangeColour(long noteId, long userId, string newColor)
        {
            try
            {
                var resNote = this.fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();
                if (resNote != null)
                {
                    resNote.Color = newColor;
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

        //Method To Upload The Image And Resturn ImageUploadResult
        public ImageUploadResult UploadImage(IFormFile imagePath)
        {
            try
            {
                Account account = new Account(configuration["Cloudinary:CloudName"], configuration["Cloudinary:ApiKey"], configuration["Cloudinary:ApiSecret"]);
                Cloudinary cloud = new Cloudinary(account);    
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imagePath.FileName, imagePath.OpenReadStream()),
                };
                var uploadImageRes = cloud.Upload(uploadParams);
                if (uploadImageRes != null)
                    return uploadImageRes;
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch The Notes Details And Update The Image In The Image Field Of Notes Using Cloudinary
        public NoteEntity UpdateImage(long noteId, long userId, IFormFile imagePath)
        {
            try
            {
                var resNote = fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();
                if (resNote != null)
                {
                    resNote.Image = UploadImage(imagePath).Url.ToString();
                    resNote.ModifiedAt = DateTime.Now;
                    fundooContext.NotesData.Update(resNote);
                    fundooContext.SaveChanges();
                    return resNote;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch The Notes Details And Delete The Image In The Image Field Of Notes Using UserId
        public string DeleteImage(long noteId, long userId)
        {
            try
            {
                var resNote = this.fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault(); ;
                if (resNote != null)
                {
                    resNote.Image = null;
                    resNote.ModifiedAt = DateTime.Now;
                    fundooContext.NotesData.Update(resNote);
                    fundooContext.SaveChanges();
                    return "Deleted The Image Successfully";
                }
                else
                    return "Deletion Of Image Failed";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
