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
        public NotesResponse CreateNote(UserNotes userNotes, long userId)
        {
            try
            {
                IEnumerable<ImageEntity> imageList = null;
                NoteEntity notesEntity = new NoteEntity
                {
                    Title = userNotes.Title,
                    Description = userNotes.Description,
                    Reminder = userNotes.Reminder,
                    Color = userNotes.Color,
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
                if(userNotes.ImagePaths.Count > 0)
                {
                    long noteId = notesEntity.NoteId;
                    imageList = AddImages(noteId, userId, userNotes.ImagePaths);
                }
                if (res > 0)
                {
                    NotesResponse notesResponse = new NotesResponse()
                    {
                        NotesDetails = notesEntity,
                        ImagesDetails = imageList,
                    };
                    return notesResponse;
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
        public GetNotesResponse GetNote(long noteId, long userId)
        {
            try
            {
                NoteEntity resultNote = null;
                var collabRes = fundooContext.CollaboratorData.FirstOrDefault(n => n.UserId == userId && n.NoteId == noteId);
                if (collabRes != null)
                    resultNote = fundooContext.NotesData.Where(n => n.NoteId == collabRes.NoteId).FirstOrDefault();
                else
                    resultNote = fundooContext.NotesData.Where(n => n.UserId == userId && n.NoteId == noteId).FirstOrDefault();
                var imageList = fundooContext.ImagesData.Where(n => n.NoteId == noteId).ToList();
                if (resultNote != null || collabRes != null)
                {
                    GetNotesResponse resNote = new GetNotesResponse
                    {
                        NotesId = resultNote.NoteId,
                        Title = resultNote.Title,
                        Description = resultNote.Description,
                        Color = resultNote.Color,
                        IsArchive = resultNote.IsArchive,
                        IsTrash = resultNote.IsTrash,
                        IsPinned = resultNote.IsPinned,
                        Reminder = resultNote.Reminder.ToString("dd-MM-yyyy hh:mm:ss tt"),
                        CreatedAt = resultNote.CreatedAt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                        ModifiedAt = resultNote.ModifiedAt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                        ImageList = imageList
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
        public IEnumerable<GetNotesResponse> GetAllNotesByUserId(long userId)
        {
            try
            {
                IEnumerable<ImageEntity> imageList = null;
                IList<GetNotesResponse> noteList = new List<GetNotesResponse>();
                List<NoteEntity> resNotesList = new List<NoteEntity>();
                NoteEntity noteRes = null;
                var collabList = fundooContext.CollaboratorData.Where(n => n.UserId == userId).ToList();
                if (collabList.Count() > 0)
                {
                    foreach (var collab in collabList)
                    {
                        noteRes = fundooContext.NotesData.Where(n => n.NoteId == collab.NoteId).FirstOrDefault();
                        resNotesList.Add(noteRes);
                    }
                }
                if (resNotesList.Count() > 0) 
                { 
                    foreach (var resultNote in resNotesList)
                    {
                        imageList = fundooContext.ImagesData.Where(n => n.NoteId == resultNote.NoteId).ToList();
                        GetNotesResponse note = new GetNotesResponse
                        {
                            NotesId = resultNote.NoteId,
                            Title = resultNote.Title,
                            Description = resultNote.Description,
                            Color = resultNote.Color,
                            IsArchive = resultNote.IsArchive,
                            IsTrash = resultNote.IsTrash,
                            IsPinned = resultNote.IsPinned,
                            Reminder = resultNote.Reminder.ToString("dd-MM-yyyy hh:mm:ss tt"),
                            CreatedAt = resultNote.CreatedAt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                            ModifiedAt = resultNote.ModifiedAt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                            ImageList = imageList
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
        public IEnumerable<GetNotesResponse> GetAllNotes()
        {
            try
            {
                IEnumerable<ImageEntity> imageList = null;
                IList<GetNotesResponse> noteList = new List<GetNotesResponse>();
                var resNotesList = fundooContext.NotesData.ToList();
                if (resNotesList.Count() > 0)
                {
                    foreach (var resultNote in resNotesList)
                    {
                        imageList = fundooContext.ImagesData.Where(n => n.NoteId == resultNote.NoteId).ToList();
                        GetNotesResponse note = new GetNotesResponse
                        {
                            NotesId = resultNote.NoteId,
                            Title = resultNote.Title,
                            Description = resultNote.Description,
                            Color = resultNote.Color,
                            IsArchive = resultNote.IsArchive,
                            IsTrash = resultNote.IsTrash,
                            IsPinned = resultNote.IsPinned,
                            Reminder = resultNote.Reminder.ToString("dd-MM-yyyy hh:mm:ss tt"),
                            CreatedAt = resultNote.CreatedAt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                            ModifiedAt = resultNote.ModifiedAt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                            ImageList = imageList
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

        //Method To Fetch Multiple Notes Details From DB Using Label Name
        public IEnumerable<GetNotesResponse> GetNotesByLabelName(string labelName)
        {
            try
            {
                IEnumerable<ImageEntity> imageList = null;
                IList<GetNotesResponse> noteList = new List<GetNotesResponse>();
                List<NoteEntity> resNotesList = new List<NoteEntity>();
                NoteEntity noteRes = null;
                var labelsList = fundooContext.LabelsData.Where(l => l.LabelName == labelName).ToList();
                if (labelsList.Count() > 0)
                {
                    foreach (var labels in labelsList)
                    {
                        noteRes = fundooContext.NotesData.Where(n => n.NoteId == labels.NoteId).FirstOrDefault();
                        resNotesList.Add(noteRes);
                    }
                }
                if (resNotesList.Count() > 0)
                {
                    foreach (var resultNote in resNotesList)
                    {
                        imageList = fundooContext.ImagesData.Where(n => n.NoteId == resultNote.NoteId).ToList();
                        GetNotesResponse note = new GetNotesResponse
                        {
                            NotesId = resultNote.NoteId,
                            Title = resultNote.Title,
                            Description = resultNote.Description,
                            Color = resultNote.Color,
                            IsArchive = resultNote.IsArchive,
                            IsTrash = resultNote.IsTrash,
                            IsPinned = resultNote.IsPinned,
                            Reminder = resultNote.Reminder.ToString("dd-MM-yyyy hh:mm:ss tt"),
                            CreatedAt = resultNote.CreatedAt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                            ModifiedAt = resultNote.ModifiedAt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                            ImageList = imageList
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
                NoteEntity resNote = null;
                var collabRes = fundooContext.CollaboratorData.FirstOrDefault(n => n.UserId == userId && n.NoteId == noteId);
                if (collabRes != null)
                    resNote = fundooContext.NotesData.Where(n => n.NoteId == collabRes.NoteId).FirstOrDefault();
                else
                    resNote = fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();
                if (resNote != null)
                {
                    resNote.Title = string.IsNullOrEmpty(noteUpdate.Title) ?  resNote.Title : noteUpdate.Title;
                    resNote.Description = string.IsNullOrEmpty(noteUpdate.Description) ? resNote.Description : noteUpdate.Description;
                    resNote.Reminder = noteUpdate.Reminder.CompareTo(resNote.Reminder) == 0 ? resNote.Reminder : noteUpdate.Reminder;
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
                NoteEntity resNote = null;
                var collabRes = fundooContext.CollaboratorData.FirstOrDefault(n => n.UserId == userId && n.NoteId == noteId);
                if (collabRes != null)
                    resNote = fundooContext.NotesData.Where(n => n.NoteId == collabRes.NoteId).FirstOrDefault();
                else
                    resNote = fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();
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
                NoteEntity resNote = null;
                var collabRes = fundooContext.CollaboratorData.FirstOrDefault(n => n.UserId == userId && n.NoteId == noteId);
                if (collabRes != null)
                    resNote = fundooContext.NotesData.Where(n => n.NoteId == collabRes.NoteId).FirstOrDefault();
                else
                    resNote = fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();  
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
                NoteEntity resNote = null;
                var collabRes = fundooContext.CollaboratorData.FirstOrDefault(n => n.UserId == userId && n.NoteId == noteId);
                if (collabRes != null)
                    resNote = fundooContext.NotesData.Where(n => n.NoteId == collabRes.NoteId).FirstOrDefault();
                else
                    resNote = this.fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();
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
        public IEnumerable<ImageEntity> AddImages(long noteId, long userId, ICollection<IFormFile> imagesPath)
        {
            try
            {
                NoteEntity resNote = null;
                var collabRes = fundooContext.CollaboratorData.FirstOrDefault(n => n.UserId == userId && n.NoteId == noteId);
                if (collabRes != null)
                    resNote = fundooContext.NotesData.Where(n => n.NoteId == collabRes.NoteId).FirstOrDefault();
                else
                    resNote = fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();
                if (resNote != null)
                {
                    IList<ImageEntity> images = new List<ImageEntity>();
                    foreach (var file in imagesPath)
                    {
                        ImageEntity imageEntity = new ImageEntity();
                        var uploadImageRes = UploadImage(file);
                        imageEntity.NoteId = noteId;
                        imageEntity.ImageUrl = uploadImageRes.Url.ToString();
                        imageEntity.ImageName = file.FileName;
                        images.Add(imageEntity);
                        fundooContext.ImagesData.Add(imageEntity);
                        fundooContext.SaveChanges();
                        resNote.ModifiedAt = DateTime.Now;
                        fundooContext.NotesData.Update(resNote);
                        fundooContext.SaveChanges();
                    }
                    return images;
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
        public string DeleteImage(long imageId, long noteId, long userId)
        {
            try
            {
                NoteEntity resNote = null;
                var collabRes = fundooContext.CollaboratorData.FirstOrDefault(n => n.UserId == userId && n.NoteId == noteId);
                if (collabRes != null)
                    resNote = fundooContext.NotesData.Where(n => n.NoteId == collabRes.NoteId).FirstOrDefault();
                else
                    resNote = this.fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();
                var imageRes = this.fundooContext.ImagesData.Where(n => n.ImageId == imageId).FirstOrDefault();
                if (resNote != null && imageRes != null)
                {
                    // Deleting And Saving The Changes In The Database For Given ImageId.
                    fundooContext.ImagesData.Remove(imageRes);
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
