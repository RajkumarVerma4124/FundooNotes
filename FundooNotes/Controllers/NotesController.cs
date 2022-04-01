using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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
        //Object Reference For Interface INotesBL
        private readonly INotesBL notesBL;

        //Constructor To Initialize The Instance Of Interface INotesBL
        public NotesController(INotesBL notesBL)
        {
            this.notesBL = notesBL;
        }

        //Post Request For Creating A New Notes For Particular User Id (POST: /api/notes/createnote)
        [HttpPost("Create")]
        public IActionResult CreateNote(UserNotes userNotes)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = this.notesBL.CreateNote(userNotes, userId);
                if (resNote != null)
                    return this.Ok(new { success = true, message = "Note Created Successfully", data = resNote });
                else
                    return this.BadRequest(new { success = false, message = "Note Creation Failed" });
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
                var resNote = this.notesBL.GetNote(notesId, userId);
                if (resNote != null)
                    return this.Ok(new { success = true, message = "Got The Note Successfully", data = resNote });
                else
                    return this.NotFound(new { success = false, message = "Note With Given Id Not Found" });
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
                var resNote = this.notesBL.GetAllNotesByUserId(userId);
                if (resNote.Count() > 0)
                    return this.Ok(new { success = true, message = "Got The Notes Successfully", data = resNote });
                else
                    return this.BadRequest(new { success = false, message = "Notes Retrieval Failed" });
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
                var resNote = this.notesBL.GetAllNotes();
                if (resNote.Count() > 0)
                    return this.Ok(new { success = true, message = "Got The Notes Successfully", data = resNote });
                else
                    return this.BadRequest(new { success = false, message = "Notes Retrieval Failed" });
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
                var resNote = this.notesBL.UpdateNote(noteUpdate, noteId, userId);
                if (resNote != null)
                    return this.Ok(new { success = true, message = "Updated The Notes Successfully", data = resNote });
                else
                    return this.NotFound(new { success = false, message = "Note With Given Id Not Found For Update" });
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
                var resNote = this.notesBL.DeleteNote(noteId, userId);
                if (string.IsNullOrEmpty(resNote))
                    return this.Ok(new { success = true, message = resNote});
                else
                    return this.NotFound(new { success = false, message = resNote });
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
                var resNote = this.notesBL.ChangeIsArchieveStatus(noteId, userId);
                if (resNote != null)
                    return this.Ok(new { Success = true, message = "Archive Status Changed Successfully", data = resNote });
                else
                    return this.NotFound(new { Success = false, message = "Archive Status Changed Failed" });
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
                var resNote = this.notesBL.ChangeIsPinnedStatus(noteId, userId);
                if (resNote != null)
                    return this.Ok(new { Success = true, message = "Pinned Status Changed Successfully", data = resNote });
                else
                    return this.NotFound(new { Success = false, message = "Pinned Status Changed Failed" });
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
                var resNote = this.notesBL.ChangeIsTrashStatus(noteId, userId);
                if (resNote != null)
                    return this.Ok(new { Success = true, message = "Trash Status Changed Successfully", data = resNote });
                else
                    return this.NotFound(new { Success = false, message = "Trash Status Changed Failed" });
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
                var resNote = this.notesBL.ChangeColour(noteId, userId, newColor);
                if (resNote != null)
                    return this.Ok(new { Success = true, message = "Note Colour Changed successfully ", data = resNote });
                else
                    return this.NotFound(new { Success = false, message = "Change Color Failed As Given Id Note Found" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Patch Request For Uploading A Image Using Cloudinary (PATCH: /api/notes/uploadimage)
        [HttpPatch("UploadImage/{noteId}")]
        public IActionResult UpdateImage(long noteId, IFormFile image)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                    return BadRequest(new { success = false, message = "Note Id Should Be Greater Than Zero" });
                var resNote = this.notesBL.UpdateImage(noteId, userId, image);
                if (resNote != null)
                    return this.Ok(new { Success = true, message = "Image Uploaded Successfully", data = resNote });
                else
                    return this.NotFound(new { Success = false, message = "Image Upload Failed " });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        //Patch Request For Deleting A Image Using Notes Id And Used Id (PATCH: /api/notes/deleteimage)
        [HttpPatch("DeleteImage")]
        public IActionResult DeleteImage(long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (noteId <= 0)
                    return BadRequest(new { success = false, message = "Note Id Should Be Greater Than Zero" });
                var resNote = this.notesBL.DeleteImage(noteId, userId);
                if (resNote != null)
                    return this.Ok(new { Success = true, message = resNote });
                else
                    return this.NotFound(new { Success = false, message = resNote });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

    }
}
