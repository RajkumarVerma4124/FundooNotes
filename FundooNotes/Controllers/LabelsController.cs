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

        //Post Request For Creating A New Label (POST: /api/Label/create)
        [HttpPost("CreateNote")]
        public IActionResult CreateNoteLabel(NotesLabel notesLabel)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var ifLabelExist = labelBL.IsLabelInNoteExist(notesLabel.LabelName, notesLabel.NoteId);
                if (ifLabelExist)
                    return Ok(new { success = false, message = "The Label Already Exists" });
                var collabRes = labelBL.CreateNoteLabel(notesLabel, userId);
                if (collabRes != null)
                    return Ok(new { Success = true, message = "Label Added In Note Successfully", data = collabRes });
                else
                    return BadRequest(new { Success = false, message = "Label Creation Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Post Request For Creating A New Label (POST: /api/Label/create)
        [HttpPost("Create")]
        public IActionResult CreateNewLabel(string labelName)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var ifLabelExist = labelBL.IsLabelExist(labelName);
                if (ifLabelExist)
                    return Ok(new { success = false, message = "The Label Already Exists" });
                var collabRes = labelBL.CreateNewLabel(labelName, userId);
                if (collabRes != null)
                    return Ok(new { Success = true, message = "Label Created Successfully", data = collabRes });
                else
                    return BadRequest(new { Success = false, message = "Label Creation Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Patch Request For Creating A New Label For Particular Note (POST: /api/Label/AddToNote)
        [HttpPatch("AddToNote")]
        public IActionResult AddLabelToNote(long labelId, long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var collabRes = labelBL.AddLabelToNote(labelId, noteId, userId);
                if (collabRes != null)
                    return Ok(new { Success = true, message = "Label Added To Note Successfully", data = collabRes });
                else
                    return BadRequest(new { Success = false, message = "Label Addition To Note Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Patch Request For Editing The Label For Particular Note (PATCH: /api/Label/Edit)
        [HttpPatch("Edit")]
        public IActionResult EditLabel(string newLableName, long labelId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var collabRes = labelBL.EditLabel(newLableName, userId, labelId); ;
                if (collabRes != null)
                    return Ok(new { Success = true, message = "Label Edited Successfully", data = collabRes });
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
