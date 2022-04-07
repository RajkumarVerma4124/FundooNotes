using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
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
        //Object Reference For Interface INotesBL,IDistributedCache,IMemoryCache
        private readonly INotesBL notesBL;
        private readonly IDistributedCache distributedCache;
        private readonly IMemoryCache memoryCache;

        //Constructor To Initialize The Instance Of Interface INotesBL
        public NotesController(INotesBL notesBL, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.notesBL = notesBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }

        //Post Request For Creating A New Notes For Particular User Id (POST: /api/notes/createnote)
        [HttpPost("Create")]
        public IActionResult CreateNote([FromForm] UserNotes userNotes)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.CreateNote(userNotes, userId);
                if (resNote != null)
                    return Ok(new { success = true, message = "Note Created Successfully", data = resNote });
                else
                    return BadRequest(new { success = false, message = "Note Creation Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Get Request For Retreiving A Single Note For Particular User Id (GET: /api/notes/getnote)
        [HttpGet("Get/{notesId}")]
        public IActionResult GetNote(long notesId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (notesId <= 0)
                    return BadRequest(new { success = false, message = "Note Id Should Be Greater Than Zero" });
                var resNote = notesBL.GetNote(notesId, userId);
                if (resNote != null)
                    return Ok(new { success = true, message = "Got The Note Successfully", data = resNote });
                else
                    return NotFound(new { success = false, message = "Note With Given Id Not Found" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Get Request For Retreiving A Multiple Notes For Particular User Id (GET: /api/notes/getusers)
        [HttpGet("GetUsers")]
        public IActionResult GetAllNotesByUserId()
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.GetAllNotesByUserId(userId);
                if (resNote != null)
                    return Ok(new { success = true, message = "Got The Notes Successfully", data = resNote });
                else
                    return BadRequest(new { success = false, message = "Notes Retrieval Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Get Request For Retreiving ALL Notes In The DB(GET: /api/notes/getall)
        [HttpGet("GetAll")]
        public IActionResult GetAllNotes()
        {
            try
            {
                var resNote = notesBL.GetAllNotes();
                if (resNote != null)
                    return Ok(new { success = true, message = "Got The Notes Successfully", data = resNote });
                else
                    return BadRequest(new { success = false, message = "Notes Retrieval Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Get Request For Retreiving A Multiple Notes For Particular Labels (GET: /api/notes/getlabels)
        [HttpGet("GetLabels")]
        public IActionResult GetNotesByLabelName(string labelName)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.GetNotesByLabelName(labelName);
                if (resNote != null)
                    return Ok(new { success = true, message = "Got The Notes Successfully", data = resNote });
                else
                    return BadRequest(new { success = false, message = "Notes Retrieval Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Put Request For Updating A Particular Notes For Particular User Id (PUT: /api/notes/updatenote)
        [HttpPut("Update")]
        public IActionResult UpdateNote(NoteUpdate noteUpdate, long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                    return BadRequest(new { success = false, message = "Note Id Should Be Greater Than Zero" });
                var resNote = notesBL.UpdateNote(noteUpdate, noteId, userId);
                if (resNote != null)
                    return Ok(new { success = true, message = "Updated The Notes Successfully", data = resNote });
                else
                    return NotFound(new { success = false, message = "Note With Given Id Not Found For Update" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Delete Request For Deleting A Particular Note For Particular User Id (PUT: /api/notes/deletenote)
        [HttpDelete("Delete")]
        public IActionResult DeleteNote(long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                    return BadRequest(new { success = false, message = "Note Id Should Be Greater Than Zero" });
                var resNote = notesBL.DeleteNote(noteId, userId);
                if (string.IsNullOrEmpty(resNote))
                    return Ok(new { success = true, message = resNote});
                else
                    return NotFound(new { success = false, message = resNote });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Patch Request For Determining Whether A Particular Note Is Archive Or Not For Particular User Id (PATCH: /api/notes/isarachiveornot)
        [HttpPatch("IsArchive")]
        public IActionResult ChangeIsArchieveStatus(long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                    return BadRequest(new { success = false, message = "Note Id Should Be Greater Than Zero" });
                var resNote = notesBL.ChangeIsArchieveStatus(noteId, userId);
                if (resNote != null)
                    return Ok(new { Success = true, message = "Archive Status Changed Successfully", data = resNote });
                else
                    return NotFound(new { Success = false, message = "Archive Status Changed Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Patch Request For Determining Whether A Particular Note Is Pinned Or Not For Particular User Id (PATCH: /api/notes/ispinnedornot)
        [HttpPatch("IsPinned")]
        public IActionResult ChangeIsPinnedStatus(long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                    return BadRequest(new { success = false, message = "Note Id Should Be Greater Than Zero" });
                var resNote = notesBL.ChangeIsPinnedStatus(noteId, userId);
                if (resNote != null)
                    return Ok(new { Success = true, message = "Pinned Status Changed Successfully", data = resNote });
                else
                    return NotFound(new { Success = false, message = "Pinned Status Changed Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Patch Request For Determining Whether A Particular Note Is Trashed Or Not For Particular User Id (PATCH: /api/notes/istrashornot)
        [HttpPatch("IsTrash")]
        public IActionResult ChangeIsTrashStatus(long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                    return BadRequest(new { success = false, message = "Note Id Should Be Greater Than Zero" });
                var resNote = notesBL.ChangeIsTrashStatus(noteId, userId);
                if (resNote != null)
                    return Ok(new { Success = true, message = "Trash Status Changed Successfully", data = resNote });
                else
                    return NotFound(new { Success = false, message = "Trash Status Changed Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Patch Request For Changing The Color Of A Particular Note Is (PATCH: /api/notes/color)
        [HttpPatch("colour")]
        public IActionResult ChangeColour(long noteId, string newColor)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                    return BadRequest(new { success = false, message = "Note Id Should Be Greater Than Zero" });
                var resNote = notesBL.ChangeColour(noteId, userId, newColor);
                if (resNote != null)
                    return Ok(new { Success = true, message = "Note Colour Changed successfully ", data = resNote });
                else
                    return NotFound(new { Success = false, message = "Change Color Failed As Given Id Note Found" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("AddImages/{noteId}")]
        public IActionResult AddImages(long noteId, ICollection<IFormFile> image)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                    return BadRequest(new { success = false, message = "Note Id Should Be Greater Than Zero" });
                var resNote = notesBL.AddImages(noteId, userId, image);
                if (resNote != null)
                    return Ok(new { Success = true, message = "Image Updated Successfully", data = resNote });
                else
                    return NotFound(new { Success = false, message = "Image Updated Failed " });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        //Patch Request For Deleting A Image Using Notes Id And Used Id (PATCH: /api/notes/deleteimage)
        [HttpPut("DeleteImage")]
        public IActionResult DeleteImage(long noteId, long imageId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                    return BadRequest(new { success = false, message = "Note Id Should Be Greater Than Zero" });
                var resNote = notesBL.DeleteImage(imageId, noteId, userId);
                if (resNote != null)
                    return Ok(new { Success = true, message = resNote });
                else
                    return NotFound(new { Success = false, message = resNote });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "notesList";
            string serializedNotesList;
            var notesList = new List<GetNotesResponse>();
            var redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                notesList = JsonConvert.DeserializeObject<List<GetNotesResponse>>(serializedNotesList);
            }
            else
            {
                notesList = notesBL.GetAllNotes().ToList();
                serializedNotesList = JsonConvert.SerializeObject(notesList);
                redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNotesList, options);
            }
            return Ok(notesList);
        }
    }
}
