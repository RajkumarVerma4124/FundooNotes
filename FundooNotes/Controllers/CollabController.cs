using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Interface;
using System;
using System.Linq;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollabController : ControllerBase
    {
        //Reference Object For Interface IUserRL
        private readonly ICollabBL collabBL;

        //Created Constructor With Dependency Injection For IUSerRL
        public CollabController(ICollabBL collabBL)
        {
            this.collabBL = collabBL;
        }

        //Post Request For Creating A New Collaborator (POST: /api/collaborator/add)
        [HttpPost("Add")]
        public IActionResult AddCollaborator(NotesCollab notesCollab)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var ifEmailExist = collabBL.IsEmailIdExist(notesCollab.CollabEmail, notesCollab.NoteId);
                if (ifEmailExist)
                    return Ok(new { success = false, message = "The Email Already Exists" });
                var collabRes = collabBL.AddCollaborator(notesCollab, userId);
                if (collabRes != null)
                    return Ok(new { Success = true, message = "Collaborator Added Successfully", data = collabRes });
                else
                    return BadRequest(new { Success = false, message = "Collab Creation Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Delete Request For Deleting A Collaborator (Delete: /api/collaborator/delete)
        [HttpDelete("Delete")]
        public IActionResult DeleteCollaborator(long collabId, long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (collabId <= 0)
                    return BadRequest(new { success = false, message = "Collab Id Should Be Greater Than Zero" });
                var collabRes = collabBL.DeleteCollaborator(collabId, noteId, userId);
                if (!collabRes.Contains("Remove"))
                    return Ok(new { Success = true,  message = collabRes });
                else
                    return Unauthorized(new { Success = false, message = collabRes });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Get Request For Retrieving List Of Collaborator (Get: /api/collaborator/get)
        [HttpGet("Get")]
        public IActionResult GetNoteCollaborators(long noteId)
        {
            try
            {
                if (noteId <= 0)
                    return BadRequest(new { success = false, message = "Note Id Should Be Greater Than Zero" });
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var collabRes = collabBL.GetNoteCollaborators(noteId, userId);
                if (collabRes != null)
                    return Ok(new { Success = true, message = "Got The Collaboraters Successfully", data = collabRes });
                else
                    return Unauthorized(new { Success = false, message = "You Dont Have Access To Those Notes", data = collabRes }); 
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
