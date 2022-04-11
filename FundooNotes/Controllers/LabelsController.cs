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
        /// <summary>
        /// Reference Object For Interface IUserRL
        /// </summary>
        private readonly ILabelBL labelBL;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<LabelsController> logger;


        /// <summary>
        /// Created Constructor With Dependency Injection For IUSerRL
        /// </summary>
        /// <param name="labelBL"></param>
        /// <param name="distributedCache"></param>
        /// <param name="logger"></param>
        public LabelsController(ILabelBL labelBL, IDistributedCache distributedCache, ILogger<LabelsController> logger)
        {
            this.labelBL = labelBL;
            this.distributedCache = distributedCache;
            this.logger = logger;
        }

        /// <summary>
        /// Post Request For Creating A New Label (POST: /api/labels/create)
        /// </summary>
        /// <param name="labelName"></param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        [HttpPost("Create")]
        public IActionResult CreateNewLabel(string labelName)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var ifLabelExist = labelBL.IsLabelExist(labelName, userId);
                if (ifLabelExist)
                {
                    logger.LogWarning("The Label Already Exists");
                    return BadRequest(new { Success = false, message = "The Label Already Exists" });
                }
                var labelRes = labelBL.CreateNewLabel(labelName, userId);
                if (labelRes != null)
                {
                    logger.LogInformation("Label Created Successfully");
                    return Ok(new { Success = true, message = "Label Created Successfully", data = labelRes });
                }
                else
                {
                    logger.LogError("Label Creation Failed");
                    throw new AppException("Label Creation Failed");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Post Request For Adding Or Creating A New Label And Add In Notes If Its Not Exist (POST: /api/labels/Addtonote)
        /// </summary>
        /// <param name="notesLabel"></param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        [HttpPost("AddToNote")]
        public IActionResult AddNoteLabel(NotesLabel notesLabel)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var labelRes = labelBL.AddNoteLabel(notesLabel, userId);
                if (labelRes != null)
                {
                    logger.LogInformation("Label Added In Note Successfully");
                    return Ok(new { Success = true, message = "Label Added In Note Successfully", data = labelRes });
                }
                else
                {
                    logger.LogError("Label Added To Note Failed");
                    throw new AppException("Label Added To Note Failed");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Patch Request For Editing The Label For Particular Note (PATCH: /api/labels/Edit)
        /// </summary>
        /// <param name="newLableName"></param>
        /// <param name="labelId"></param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        [HttpPatch("Edit")]
        public IActionResult EditLabel(string newLableName, long labelId)
        {
            try
            {
                if (labelId <= 0)
                {
                    logger.LogWarning("Label Id Should Be Greater Than Zero");
                    throw new AppException("Label Id Should Be Greater Than Zero");
                }
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var labelRes = labelBL.EditLabel(newLableName, userId, labelId); ;
                if (labelRes != null)
                {
                    logger.LogInformation("Label Edited Successfully");
                    return Ok(new { Success = true, message = "Label Edited Successfully", data = labelRes });
                }
                else
                {
                    logger.LogError("Label Edition Failed");
                    throw new KeyNotFoundException("Label Edition Failed");
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
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Delete Request For Removing A Label From Particular Note(Delete: /api/labels/remove)
        /// </summary>
        /// <param name="labelId"></param>
        /// <param name="noteId"></param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        [HttpDelete("Remove")]
        public IActionResult RemoveLabel(long labelId, long noteId)
        {
            try
            {
                if (labelId <= 0)
                {
                    logger.LogWarning("Label Id Should Be Greater Than Zero");
                    throw new AppException("Label Id Should Be Greater Than Zero");
                }
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var labelRes = labelBL.RemoveLabel(labelId, noteId,userId);
                if (labelRes.Contains("Removed"))
                {
                    logger.LogInformation(labelRes);
                    return Ok(new { Success = true, message = labelRes });
                }
                else
                {
                    logger.LogError(labelRes);
                    throw new UnauthorizedAccessException(labelRes);
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return Unauthorized(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Delete Request For Removing A Label From Particular Note(Delete: /api/labels/remove)
        /// </summary>
        /// <param name="labelNameId"></param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
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
                {
                    logger.LogInformation(labelRes);
                    return Ok(new { Success = true, message = labelRes });
                }
                else
                {
                    logger.LogError(labelRes);
                    throw new AppException(labelRes);
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get Request For Retrieving List Of Notes Labels (Get: /api/labels/get)
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        [HttpGet("GetNotes")]
        public IActionResult GetNoteLabels(long noteId)
        {
            try
            {
                if (noteId <= 0)
                {
                    logger.LogWarning("Note Id Should Be Greater Than Zero");
                    throw new AppException("Note Id Should Be Greater Than Zero");
                }
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var labelRes = labelBL.GetNotesLabels(noteId, userId);
                if (labelRes != null)
                {
                    logger.LogInformation("Got The Notes Label Successfully");
                    return Ok(new { Success = true, message = "Got The Notes Label Successfully", data = labelRes });
                }
                else
                {
                    logger.LogError("Notes Label Retreival Failed");
                    throw new UnauthorizedAccessException("Notes Label Retreival Failed");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return Unauthorized(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get Request For Retrieving List Of Labels (Get: /api/labels/getall)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        [HttpGet("GetAll")]
        public IActionResult GetUsersLabelsList()
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var labelRes = labelBL.GetUsersLabelsList(userId);
                if (labelRes != null)
                {
                    logger.LogInformation("Got The Label Successfully");
                    return Ok(new { Success = true, message = "Got The Label Successfully", data = labelRes });
                }
                else
                {
                    logger.LogError("Label Retreival Failed");
                    throw new AppException("Label Retreival Failed");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get Request For Retrieving List Of Labels (Get: /api/labels/getall)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        [HttpGet("GetNames")]
        public IActionResult GetUsersLabelNamesList()
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var labelRes = labelBL.GetUsersLabelNamesList(userId);
                if (labelRes != null)
                {
                    logger.LogInformation("Got The Label Names Successfully");
                    return Ok(new { Success = true, message = "Got The Label Names Successfully", data = labelRes });
                }
                else
                {
                    logger.LogError("Label Names Retreival Failed");
                    throw new AppException("Label Names Retreival Failed");
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                throw new AppException(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get Request For Retrieving List Of Labels Using Redis(Get: /api/labels/redis)
        /// </summary>
        /// <returns></returns>
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllLabelUsingRedisCache()
        {
            try
            {
                var cacheKey = "labelsList";
                string serializedLabelList;
                var labelsList = new List<LabelsResponse>();
                var redisLabelsList = await distributedCache.GetAsync(cacheKey);
                if (redisLabelsList != null)
                {
                    logger.LogDebug("Getting The List From Redis Cache");
                    serializedLabelList = Encoding.UTF8.GetString(redisLabelsList);
                    labelsList = JsonConvert.DeserializeObject<List<LabelsResponse>>(serializedLabelList);
                }
                else
                {
                    logger.LogDebug("Setting The Labels List To Cache Which Request For First Time");
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
                logger.LogInformation("Got The Labels List Successfully Using Redis");
                return Ok(labelsList);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
