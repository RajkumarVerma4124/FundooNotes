using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabelsController : ControllerBase
    {
        //Reference Object For Interface IUserRL
        private readonly ILabelBL labelBL;
        private readonly IDistributedCache distributedCache;
        private readonly IMemoryCache memoryCache;

        //Created Constructor With Dependency Injection For IUSerRL
        public LabelsController(ILabelBL labelBL, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.labelBL = labelBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }

        //Post Request For Creating A New Label (POST: /api/labels/create)
        [HttpPost("Create")]
        public IActionResult CreateNewLabel(string labelName)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var ifLabelExist = labelBL.IsLabelExist(labelName, userId);
                if (ifLabelExist)
                    return Ok(new { success = false, message = "The Label Already Exists" });
                var labelRes = labelBL.CreateNewLabel(labelName, userId);
                if (labelRes != null)
                    return Ok(new { Success = true, message = "Label Created Successfully", data = labelRes });
                else
                    return BadRequest(new { Success = false, message = "Label Creation Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Post Request For Creating A New Label In Notes Its Not Exist (POST: /api/labels/create)
        [HttpPost("AddToNote")]
        public IActionResult AddNoteLabel(NotesLabel notesLabel)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var labelRes = labelBL.AddNoteLabel(notesLabel, userId);
                if (labelRes != null)
                    return Ok(new { Success = true, message = "Label Added In Note Successfully", data = labelRes });
                else
                    return BadRequest(new { Success = false, message = "Label Creation Failed" });  
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Patch Request For Editing The Label For Particular Note (PATCH: /api/labels/Edit)
        [HttpPatch("Edit")]
        public IActionResult EditLabel(string newLableName, long labelId)
        {
            try
            {
                if (labelId <= 0)
                    return BadRequest(new { success = false, message = "Label Id Should Be Greater Than Zero" });
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var labelRes = labelBL.EditLabel(newLableName, userId, labelId); ;
                if (labelRes != null)
                    return Ok(new { Success = true, message = "Label Edited Successfully", data = labelRes });
                else
                    return BadRequest(new { Success = false, message = "Label Creation Failed" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Delete Request For Removing A Label From Particular Note(Delete: /api/labels/remove)
        [HttpDelete("Remove")]
        public IActionResult RemoveLabel(long labelId, long noteId)
        {
            try
            {
                if (labelId <= 0)
                    return BadRequest(new { success = false, message = "Label Id Should Be Greater Than Zero" });
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var labelRes = labelBL.RemoveLabel(labelId, noteId,userId);
                if (labelRes.Contains("Removed"))
                    return Ok(new { Success = true, message = labelRes });
                else
                    return Unauthorized(new { Success = false, message = labelRes });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Delete Request For Deleting A Label (Delete: /api/labels/delete)
        [HttpDelete("Delete")]
        public IActionResult DeleteLabel(long labelNameId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var labelRes = labelBL.DeleteLabel(labelNameId, userId);
                if (labelRes.Contains("Deleted"))
                    return Ok(new { Success = true, message = labelRes });
                else
                    return BadRequest(new { Success = false, message = labelRes });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Get Request For Retrieving List Of Notes Labels (Get: /api/labels/get)
        [HttpGet("GetNotes")]
        public IActionResult GetNoteLabels(long noteId)
        {
            try
            {
                if (noteId <= 0)
                    return BadRequest(new { success = false, message = "Note Id Should Be Greater Than Zero" });
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var labelRes = labelBL.GetNotesLabels(noteId, userId);
                if (labelRes != null)
                    return Ok(new { Success = true, message = "Got The Notes Label Successfully", data = labelRes });
                else
                    return Unauthorized(new { Success = false, message = "Notes Label Retreival Failed", data = labelRes });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Get Request For Retrieving List Of Labels (Get: /api/labels/getall)
        [HttpGet("GetAll")]
        public IActionResult GetUsersLabelsList()
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var labelRes = labelBL.GetUsersLabelsList(userId);
                if (labelRes != null)
                    return Ok(new { Success = true, message = "Got The Label Successfully", data = labelRes });
                else
                    return Unauthorized(new { Success = false, message = "Label Retreival Failed", data = labelRes });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Get Request For Retrieving List Of Labels (Get: /api/labels/getall)
        [HttpGet("GetNames")]
        public IActionResult GetUsersLabelNamesList()
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var labelRes = labelBL.GetUsersLabelNamesList(userId);
                if (labelRes != null)
                    return Ok(new { Success = true, message = "Got The Label Names Successfully", data = labelRes });
                else
                    return Unauthorized(new { Success = false, message = "Label Nams Retreival Failed", data = labelRes });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("redis")]
        public async Task<IActionResult> GetAllLabelUsingRedisCache()
        {
            var cacheKey = "labelsList";
            string serializedLabelList;
            var labelsList = new List<LabelsResponse>();
            var redisLabelsList = await distributedCache.GetAsync(cacheKey);
            if (redisLabelsList != null)
            {
                serializedLabelList = Encoding.UTF8.GetString(redisLabelsList);
                labelsList = JsonConvert.DeserializeObject<List<LabelsResponse>>(serializedLabelList);
            }
            else
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                labelsList = labelBL.GetUsersLabelsList(userId).ToList();
                serializedLabelList = JsonConvert.SerializeObject(labelsList);
                redisLabelsList = Encoding.UTF8.GetBytes(serializedLabelList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisLabelsList, options);
            }
            return Ok(labelsList);
        }
    }
}
