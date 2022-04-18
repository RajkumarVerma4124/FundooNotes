using BusinessLayer.Interface;
using CommonLayer.Models;
using FundooNotes.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    /// <summary>
    /// Created The Notes Controller For Http Requests And Response 👈 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface INotesBL,IDistributedCache,IMemoryCache,ILogger
        /// </summary>
        private readonly INotesBL notesBL;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<NotesController> logger;

        /// <summary>
        /// Constructor To Initialize The Instance Of Interface INotesBL
        /// </summary>
        /// <param name="notesBL"></param>
        /// <param name="distributedCache"></param>
        /// <param name="logger"></param>
        public NotesController(INotesBL notesBL, IDistributedCache distributedCache, ILogger<NotesController> logger)
        {
            this.notesBL = notesBL;
            this.distributedCache = distributedCache;
            this.logger = logger;
        }

        /// <summary>
        /// Post Request For Creating A New Notes For Particular User Id (POST: /api/notes/createnote)
        /// </summary>
        /// <param name="userNotes"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public IActionResult CreateNote([FromForm] UserNotes userNotes)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.CreateNote(userNotes, userId);
                if (resNote != null)
                {
                    logger.LogInformation("Note Created Successfully");
                    return Ok(new { success = true, message = "Note Created Successfully", data = resNote });
                }
                else
                {
                    logger.LogError("Note Creation Failed");
                    throw new AppException("Note Creation Failed Due To Improper Values");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(" Exception Thrown ..", ex.Message);
                throw new AppException(ex.Message);
            }
        }

        /// <summary>
        /// Get Request For Retreiving A Single Note For Particular User Id (GET: /api/notes/getnote)
        /// </summary>
        /// <param name="notesId"></param>
        /// <returns></returns>
        [HttpGet("Get/{notesId}")]
        public IActionResult GetNote(long notesId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (notesId <= 0)
                {
                    logger.LogWarning("Note Id Should Be Greater Than Zero");
                    throw new AppException("Note Id Should Be Greater Than Zero");
                }
                var resNote = notesBL.GetNote(notesId, userId);
                if (resNote != null)
                {
                    logger.LogInformation("Got The Note Successfully");
                    return Ok(new { success = true, message = "Got The Note Successfully", data = resNote });
                }
                else
                {
                    logger.LogError("Note With Given Id Not Found");
                    throw new KeyNotFoundException("Note With Given Id Not Found");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new KeyNotFoundException(ex.Message);
            }
        }

        /// <summary>
        /// Get Request For Retreiving A Multiple Notes For Particular User Id (GET: /api/notes/getusers)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUsers")]
        public IActionResult GetAllNotesByUserId()
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.GetAllNotesByUserId(userId);
                if (resNote != null)
                {
                    logger.LogInformation("Got The Note Successfully");
                    return Ok(new { success = true, message = "Got The Notes Successfully", data = resNote });
                }
                else
                {
                    logger.LogError("Notes Retrieval Failed");
                    throw new AppException("Notes Retrieval Failed");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
        }

        /// <summary>
        /// Get Request For Retreiving ALL Notes In The DB(GET: /api/notes/getall)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult GetAllNotes()
        {
            try
            {
                var resNote = notesBL.GetAllNotes();
                if (resNote != null)
                {
                    logger.LogInformation("Got All The Note Successfully");
                    return Ok(new { success = true, message = "Got All The Notes Successfully", data = resNote });
                }
                else
                {
                    logger.LogError("Notes Retrieval Failed");
                    throw new AppException("Notes Retrieval Failed");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
        }

        /// <summary>
        /// Get Request For Retreiving A Multiple Notes For Particular Labels (GET: /api/notes/getlabels)
        /// </summary>
        /// <param name="labelId"></param>
        /// <returns></returns>
        [HttpGet("GetLabels")]
        public IActionResult GetNotesByLabelId(long labelId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.GetNotesByLabelId(labelId);
                if (resNote != null)
                {
                    logger.LogInformation("Got The Notes Using Label Id Successfully");
                    return Ok(new { success = true, message = "Got The Notes Using Label Id Successfully", data = resNote });
                }
                else
                {
                    logger.LogError("Notes Retrieval Failed");
                    throw new AppException("Notes Retrieval Failed");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
        }

        /// <summary>
        /// Put Request For Updating A Particular Notes For Particular User Id (PUT: /api/notes/updatenote)
        /// </summary>
        /// <param name="noteUpdate"></param>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public IActionResult UpdateNote(NoteUpdate noteUpdate, long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                {
                    logger.LogWarning("Note Id Should Be Greater Than Zero");
                    throw new AppException("Note Id Should Be Greater Than Zero");
                }
                var resNote = notesBL.UpdateNote(noteUpdate, noteId, userId);
                if (resNote != null)
                {
                    logger.LogInformation("Updated The Notes Successfully");
                    return Ok(new { success = true, message = "Updated The Notes Successfully", data = resNote });
                }
                else
                {
                    logger.LogError("Note With Given Id Not Found For Update");
                    throw new KeyNotFoundException("Note With Given Id Not Found For Update");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new KeyNotFoundException(ex.Message);
            }
        }

        /// <summary>
        /// Delete Request For Deleting A Particular Note For Particular User Id (PUT: /api/notes/deletenote)
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public IActionResult DeleteNote(long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                {
                    logger.LogWarning("Note Id Should Be Greater Than Zero");
                    throw new AppException("Note Id Should Be Greater Than Zero");
                }
                var resNote = notesBL.DeleteNote(noteId, userId);
                if (!string.IsNullOrEmpty(resNote))
                {
                    logger.LogInformation(resNote);
                    return Ok(new { success = true, message = resNote });
                }
                else
                {
                    logger.LogError(resNote);
                    throw new KeyNotFoundException(resNote);
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new KeyNotFoundException(ex.Message);
            }
        }

        /// <summary>
        /// Patch Request For Determining Whether A Particular Note Is Archive Or Not For Particular User Id (PATCH: /api/notes/isarachiveornot)
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [HttpPatch("IsArchive")]
        public IActionResult ChangeIsArchieveStatus(long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                {
                    logger.LogWarning("Note Id Should Be Greater Than Zero");
                    throw new AppException("Note Id Should Be Greater Than Zero");
                }
                var resNote = notesBL.ChangeIsArchieveStatus(noteId, userId);
                if (resNote != null)
                {
                    logger.LogInformation("Archive Status Changed Successfully");
                    return Ok(new { Success = true, message = "Archive Status Changed Successfully", data = resNote });
                }
                else
                {
                    logger.LogError("Archive Status Changed Failed");
                    throw new KeyNotFoundException("Archive Status Changed Failed");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new KeyNotFoundException(ex.Message);
            }
        }

        /// <summary>
        /// Patch Request For Determining Whether A Particular Note Is Pinned Or Not For Particular User Id (PATCH: /api/notes/ispinnedornot)
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [HttpPatch("IsPinned")]
        public IActionResult ChangeIsPinnedStatus(long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                {
                    logger.LogWarning("Note Id Should Be Greater Than Zero");
                    throw new AppException("Note Id Should Be Greater Than Zero");
                }
                var resNote = notesBL.ChangeIsPinnedStatus(noteId, userId);
                if (resNote != null)
                {
                    logger.LogInformation("Pinned Status Changed Successfully");
                    return Ok(new { Success = true, message = "Pinned Status Changed Successfully", data = resNote });
                }
                else
                {
                    logger.LogError("Pinned Status Changed Failed");
                    throw new KeyNotFoundException("Pinned Status Changed Failed");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new KeyNotFoundException(ex.Message);
            }
        }

        /// <summary>
        /// Patch Request For Determining Whether A Particular Note Is Trashed Or Not For Particular User Id (PATCH: /api/notes/istrashornot)
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [HttpPatch("IsTrash")]
        public IActionResult ChangeIsTrashStatus(long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                {
                    logger.LogWarning("Note Id Should Be Greater Than Zero");
                    throw new AppException("Note Id Should Be Greater Than Zero");
                }
                var resNote = notesBL.ChangeIsTrashStatus(noteId, userId);
                if (resNote != null)
                {
                    logger.LogInformation("Trash Status Changed Successfully");
                    return Ok(new { Success = true, message = "Trash Status Changed Successfully", data = resNote });
                }
                else
                {
                    logger.LogError("Trash Status Changed Failed");
                    throw new KeyNotFoundException("Trash Status Changed Failed");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw ex;
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw ex;
            }
        }

        /// <summary>
        /// Patch Request For Changing The Color Of A Particular Note Is (PATCH: /api/notes/color)
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="newColor"></param>
        /// <returns></returns>
        [HttpPatch("colour")]
        public IActionResult ChangeColour(long noteId, string newColor)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                {
                    logger.LogWarning("Note Id Should Be Greater Than Zero");
                    throw new AppException("Note Id Should Be Greater Than Zero");
                }
                var resNote = notesBL.ChangeColour(noteId, userId, newColor);
                if (resNote != null)
                {
                    logger.LogInformation("Note Colour Changed Successfully");
                    return Ok(new { Success = true, message = "Note Colour Changed successfully ", data = resNote });
                }
                else
                {
                    logger.LogError("Change Color Failed As Given Id Note Found");
                    throw new KeyNotFoundException("Change Color Failed As Given Id Note Found");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new KeyNotFoundException(ex.Message);
            }   
        }

        /// <summary>
        /// Post Request For Adding Multiple Images Using Notes Id And Used Id (POST: /api/notes/addimage)
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost("AddImages/{noteId}")]
        public IActionResult AddImages(long noteId, ICollection<IFormFile> image)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                {
                    logger.LogWarning("Note Id Should Be Greater Than Zero");
                    throw new AppException("Note Id Should Be Greater Than Zero");
                }
                var resNote = notesBL.AddImages(noteId, userId, image);
                if (resNote != null)
                {
                    logger.LogInformation("Image Updated Successfully");
                    return Ok(new { Success = true, message = "Image Updated Successfully", data = resNote });
                }
                else
                {
                    logger.LogError("Image Updation Failed");
                    throw new KeyNotFoundException("Image Updation Failed");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new KeyNotFoundException(ex.Message);
            }
        }

        /// <summary>
        /// Put Request For Deleting A Image Using Notes Id And Used Id (PUT: /api/notes/deleteimage)
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [HttpPut("DeleteImage")]
        public IActionResult DeleteImage(long noteId, long imageId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                {
                    logger.LogWarning("Note Id Should Be Greater Than Zero");
                    throw new AppException("Note Id Should Be Greater Than Zero");
                }
                var resNote = notesBL.DeleteImage(imageId, noteId, userId);
                if (resNote != null)
                {
                    logger.LogInformation(resNote);
                    return Ok(new { Success = true, message = resNote });
                }
                else
                {
                    logger.LogError(resNote);
                    throw new KeyNotFoundException(resNote);
                }
            }
            catch(AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new KeyNotFoundException(ex.Message);
            }
        }

        /// <summary>
        /// Get Request For Retreiving ALL Notes Using Redis(GET: /api/notes/redis)
        /// </summary>
        /// <returns></returns>
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            try
            {
                var cacheKey = "notesList";
                string serializedNotesList;
                var notesList = new List<GetNotesResponse>();
                var redisNotesList = await distributedCache.GetAsync(cacheKey);
                if (redisNotesList != null)
                {
                    logger.LogDebug("Getting The List From Redis Cache");
                    serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                    notesList = JsonConvert.DeserializeObject<List<GetNotesResponse>>(serializedNotesList);
                }
                else
                {
                    logger.LogDebug("Setting The Notes List To Cache Which Request For First Time");
                    notesList = notesBL.GetAllNotes().ToList();
                    serializedNotesList = JsonConvert.SerializeObject(notesList);
                    redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey, redisNotesList, options);
                }
                logger.LogInformation("Got The NotesList Successfully Using Redis");
                return Ok(notesList);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
