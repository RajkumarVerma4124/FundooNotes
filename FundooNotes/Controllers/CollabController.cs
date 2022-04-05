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
    }
}
