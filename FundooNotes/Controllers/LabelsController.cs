using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabelsController : ControllerBase
    {
        //Reference Object For Interface IUserRL
        private readonly ILabelBL labelBL;

        //Created Constructor With Dependency Injection For IUSerRL
        public LabelsController(ILabelBL labelBL)
        {
            this.labelBL = labelBL;
        }

        //Post Request For Creating A New Label For Particular Note (POST: /api/collaborator/add)
        [HttpPost("Add")]
        public IActionResult CreateLabel(NotesLabel notesLabel)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var ifLabelExist = labelBL.IsLabelExist(notesLabel.LabelName, notesLabel.NoteId);
                if (ifLabelExist)
                    return Ok(new { success = false, message = "The Label Already Exists" });
                var collabRes = labelBL.CreateLabel(notesLabel, userId);
                if (collabRes != null)
                    return Ok(new { Success = true, message = "Label Added Successfully", data = collabRes });
                else
                    return BadRequest(new { Success = false, message = "Label Creation Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
