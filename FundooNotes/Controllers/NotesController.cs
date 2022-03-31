﻿using BusinessLayer.Interface;
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
        [HttpPost("CreateNote")]
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
        [HttpGet("GetNote/notesId")]
        public IActionResult GetNote(long notesId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = this.notesBL.GetNote(notesId, userId);
                if (resNote != null)
                    return this.Ok(new { success = true, message = "Got The Note Successfully", data = resNote });
                else
                    return this.BadRequest(new { success = false, message = "Note Retrieval Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Get Request For Retreiving A Multiple Notes For Particular User Id (GET: /api/notes/getallnote)
        [HttpGet("GetAllNotes")]
        public IActionResult GetAllNotesByUserId()
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = this.notesBL.GetAllNotesByUserId(userId);
                if (resNote != null)
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
                if (resNote != null)
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
        [HttpPut("UpdateNote")]
        public IActionResult UpdateNote(NoteUpdate noteUpdate, long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = this.notesBL.UpdateNote(noteUpdate, noteId, userId);
                if (resNote != null)
                    return this.Ok(new { success = true, message = "Updated The Notes Successfully", data = resNote });
                else
                    return this.BadRequest(new { success = false, message = "Notes Updation Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Delete Request For Deleting A Particular Note For Particular User Id (PUT: /api/notes/deletenote)
        [HttpDelete("DeleteNote")]
        public IActionResult DeleteNote(long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = this.notesBL.DeleteNote(noteId, userId);
                if (string.IsNullOrEmpty(resNote))
                    return this.Ok(new { success = true, message = resNote});
                else
                    return this.BadRequest(new { success = false, message = resNote });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Patch Request For Determining Whether A Particular Note Is Archive Or Not For Particular User Id (PATCH: /api/notes/isarachiveornot)
        [HttpPatch("IsArchiveOrNot")]
        public IActionResult CheckIsArchieveOrNot(long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = this.notesBL.CheckIsArchieveOrNot(noteId, userId);
                if (resNote != null)
                    return this.Ok(new { Success = true, message = "Archive Status Changed Successfully", data = resNote });
                else
                    return this.BadRequest(new { Success = false, message = "Archive Status Changed Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Patch Request For Determining Whether A Particular Note Is Pinned Or Not For Particular User Id (PATCH: /api/notes/ispinnedornot)
        [HttpPatch("IsPinnedOrNot")]
        public IActionResult CheckIsPinnedOrNot(long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = this.notesBL.CheckIsPinnedOrNot(noteId, userId);
                if (resNote != null)
                    return this.Ok(new { Success = true, message = "Archive Status Changed Successfully", data = resNote });
                else
                    return this.BadRequest(new { Success = false, message = "Archive Status Changed Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Patch Request For Determining Whether A Particular Note Is Trashed Or Not For Particular User Id (PATCH: /api/notes/istrashornot)
        [HttpPatch("IsTrashOrNot")]
        public IActionResult CheckIsTrashOrNot(long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = this.notesBL.CheckIsTrashOrNot(noteId, userId);
                if (resNote != null)
                    return this.Ok(new { Success = true, message = "Archive Status Changed Successfully", data = resNote });
                else
                    return this.BadRequest(new { Success = false, message = "Archive Status Changed Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}