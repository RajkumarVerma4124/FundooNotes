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
        /// <summary>
        /// Reference Object For FundooContext And IConfiguaration
        /// </summary>
        private readonly FundooContext fundooContext;
        private readonly IConfiguration configuration;


        /// <summary>
        /// Created Constructor To Initialize Fundoocontext For Each Instance
        /// </summary>
        /// <param name="fundooContext"></param>
        /// <param name="configuration"></param>
        public NotesRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
        }

        /// <summary>
        /// Method To Create Note With New Notes Data And Userid Into The Db Table
        /// </summary>
        /// <param name="userNotes"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public NotesResponse CreateNote(UserNotes userNotes, long userId)
        {
            try
            {
                if (userNotes != null)
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
                    //Condition For Adding Multiple Images If Provide While Creating Note
                    if (userNotes.ImagePaths.Count > 0)
                    {
                        imageList = AddImages(notesEntity.NoteId, userId, userNotes.ImagePaths);
                    }
                    /*//Calling The Method To Add Owner In Collab While Creating Note
                    var resOwnerCollab = AddOwner(notesEntity.NoteId, userId);*/
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
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Fetch Single Note Details By Giving Note And User Ids
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public GetNotesResponse GetNote(long noteId, long userId)
        {
            try
            {
                NoteEntity resultNote = null;
                CollabUserNotesEntity collabUserNote = null;
                var collabRes = fundooContext.CollaboratorData.FirstOrDefault(n => n.UserId == userId && n.NoteId == noteId);
                if (collabRes != null)
                    resultNote = fundooContext.NotesData.Where(n => n.NoteId == collabRes.NoteId).FirstOrDefault();
                else
                    resultNote = fundooContext.NotesData.Where(n => n.UserId == userId && n.NoteId == noteId).FirstOrDefault();
                var imageList = fundooContext.ImagesData.Where(n => n.NoteId == noteId).ToList();
                if (resultNote != null)
                {
                    if(collabRes != null)
                        collabUserNote = fundooContext.CollabUserNotesData.FirstOrDefault(n => n.UserId == collabRes.UserId && n.CollabId == collabRes.CollabId);
                    GetNotesResponse resNote = new GetNotesResponse
                    {
                        NotesId = resultNote.NoteId,
                        Title = resultNote.Title,
                        Description = resultNote.Description,
                        Color = collabUserNote != null ? collabUserNote.NoteColor : resultNote.Color,
                        IsArchive = collabUserNote != null ? collabUserNote.IsArchive : resultNote.IsArchive,
                        IsTrash = resultNote.IsTrash,
                        IsPinned = collabUserNote != null ? collabUserNote.IsPinned : resultNote.IsPinned,
                        Reminder = collabUserNote != null ? collabUserNote.Reminder.Value.ToString("dd-MM-yyyy hh:mm:ss tt") : resultNote.Reminder.ToString("dd-MM-yyyy hh:mm:ss tt"),
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

        /// <summary>
        /// Method To Fetch Multiple Notes Details By User Ids
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<GetNotesResponse> GetAllNotesByUserId(long userId)
        {
            try
            {
                IEnumerable<ImageEntity> imageList = null;
                CollabUserNotesEntity collabUserNote = null;
                IList<GetNotesResponse> noteList = new List<GetNotesResponse>();
                List<NoteEntity> resCollabList = new List<NoteEntity>();
                List<NoteEntity> resNotesList = new List<NoteEntity>();
                var userNotesList = fundooContext.NotesData.Where(n => n.UserId == userId).ToList();
                resNotesList.AddRange(userNotesList);
                var collabList = fundooContext.CollaboratorData.Where(n => n.UserId == userId).ToList();
                if (collabList.Count() > 0)
                {
                    foreach (var collab in collabList)
                    {
                        var noteRes = fundooContext.NotesData.FirstOrDefault(n => n.NoteId == collab.NoteId);
                        if (noteRes != null)
                            resNotesList.Add(noteRes);
                    }
                }
                if (resNotesList.Count() > 0) 
                { 
                    foreach (var resultNote in resNotesList)
                    {
                        imageList = fundooContext.ImagesData.Where(n => n.NoteId == resultNote.NoteId).ToList();
                        var collabNote = fundooContext.CollaboratorData.FirstOrDefault(n => n.UserId == userId && n.NoteId == resultNote.NoteId);
                        if(collabNote != null)
                            collabUserNote = fundooContext.CollabUserNotesData.FirstOrDefault(n => n.UserId == collabNote.UserId && n.CollabId == collabNote.CollabId);
                        GetNotesResponse note = new GetNotesResponse
                        {
                            NotesId = resultNote.NoteId,
                            Title = resultNote.Title,
                            Description = resultNote.Description,
                            Color = collabUserNote != null ? collabUserNote.NoteColor : resultNote.Color,
                            IsArchive = collabUserNote != null ? collabUserNote.IsArchive : resultNote.IsArchive,
                            IsTrash = resultNote.IsTrash,
                            IsPinned = collabUserNote != null ? collabUserNote.IsPinned : resultNote.IsPinned,
                            Reminder = collabUserNote != null ? collabUserNote.Reminder.Value.ToString("dd-MM-yyyy hh:mm:ss tt") : resultNote.Reminder.ToString("dd-MM-yyyy hh:mm:ss tt"),
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

        /// <summary>
        /// Method To Fetch Multiple Notes Details From DB
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Fetch Multiple Notes Details From DB Using Label Name
        /// </summary>
        /// <param name="labelNameId"></param>
        /// <returns></returns>
        public IEnumerable<GetNotesResponse> GetNotesByLabelId(long labelNameId)
        {
            try
            {
                IEnumerable<ImageEntity> imageList = null;
                IList<GetNotesResponse> noteList = new List<GetNotesResponse>();
                List<NoteEntity> resNotesList = new List<NoteEntity>();
                NoteEntity noteRes = null;
                var labelsList = fundooContext.LabelsData.Where(l => l.LabelNameId == labelNameId).ToList();
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

        /// <summary>
        /// Method To Fetch And Update Notes Details And SaveChanges In DB
        /// </summary>
        /// <param name="noteUpdate"></param>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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
                    if(collabRes != null)
                    {
                        var collabUserNote = fundooContext.CollabUserNotesData.FirstOrDefault(n => n.UserId == collabRes.UserId && n.CollabId == collabRes.CollabId);
                        collabUserNote.Reminder = noteUpdate.Reminder.CompareTo(collabUserNote.Reminder) == 0 ? collabUserNote.Reminder : noteUpdate.Reminder;
                    }
                    else
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

        /// <summary>
        /// Method To Fetch And Delete Note Using Note Id
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Fetch The Notes Details And Turn IsArchive If True Then False And Vice Versa
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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
                    if(collabRes != null)
                    {
                        var collabUserNote = fundooContext.CollabUserNotesData.FirstOrDefault(n => n.UserId == collabRes.UserId && n.CollabId == collabRes.CollabId);
                        if (collabUserNote.IsArchive == false)
                            collabUserNote.IsArchive = true;
                        else
                            collabUserNote.IsArchive = false;
                    }
                    else
                    {
                        if (resNote.IsArchive == false)
                            resNote.IsArchive = true;
                        else
                            resNote.IsArchive = false;
                    }
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

        /// <summary>
        /// Method To Fetch The Notes Details And Turn IsPinned If True Then False And Vice Versa
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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
                    if (collabRes != null)
                    {
                        var collabUserNote = fundooContext.CollabUserNotesData.FirstOrDefault(n => n.UserId == collabRes.UserId && n.CollabId == collabRes.CollabId);
                        if (collabUserNote.IsPinned == false)
                            collabUserNote.IsPinned = true;
                        else
                            collabUserNote.IsPinned = false;
                    }
                    else
                    {
                        if (resNote.IsPinned == false)
                            resNote.IsPinned = true;
                        else
                            resNote.IsPinned = false;
                    }
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

        /// <summary>
        /// Method To Fetch The Notes Details And Turn IsTrash If True Then False And Vice Versa
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Fetch The Notes Details And Changed The Color Of The Note
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <param name="newColor"></param>
        /// <returns></returns>
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
                    if (collabRes != null)
                    {
                        var collabUserNote = fundooContext.CollabUserNotesData.FirstOrDefault(n => n.UserId == collabRes.UserId && n.CollabId == collabRes.CollabId);
                        collabUserNote.NoteColor = newColor;
                    }
                    else
                    {
                        resNote.Color = newColor;
                    }
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

        /// <summary>
        /// Method To Upload The Image And Resturn ImageUploadResult
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Fetch The Notes Details And Update The Image In The Image Field Of Notes Using Cloudinary
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <param name="imagesPath"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Fetch The Notes Details And Delete The Image In The Image Field Of Notes Using UserId
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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
