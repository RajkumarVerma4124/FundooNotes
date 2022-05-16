using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Microsoft.Extensions.Logging;
using FundooNotes.Helpers;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollabController : ControllerBase
    {
        /// <summary>
        /// Reference Object For Interface IUserRL,IDistributedCache,IMemoryCache,ILogger
        /// </summary>
        private readonly ICollabBL collabBL;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<CollabController> logger;

        /// <summary>
        /// Created Constructor With Dependency Injection For IUSerRL
        /// </summary>
        /// <param name="collabBL"></param>
        /// <param name="distributedCache"></param>
        /// <param name="logger"></param>
        public CollabController(ICollabBL collabBL, IDistributedCache distributedCache, ILogger<CollabController> logger)
        {
            this.collabBL = collabBL;
            this.distributedCache = distributedCache;
            this.logger = logger;
        }

        /// <summary>
        /// Post Request For Creating A New Collaborator (POST: /api/collaborator/add)
        /// </summary>
        /// <param name="notesCollab"></param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        [HttpPost("Add")]
        public IActionResult AddCollaborator(NotesCollab notesCollab)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var ifEmailExist = collabBL.IsEmailIdExist(notesCollab.CollabEmail, notesCollab.NoteId, userId);
                if (ifEmailExist)
                {
                    logger.LogWarning("The Email Already Exists");
                    return BadRequest(new { success = false, message = "The Email Already Exists" });
                }
                var collabRes = collabBL.AddCollaborator(notesCollab, userId);
                if (collabRes != null)
                {
                    logger.LogInformation("Collaborator Added Successfully");
                    return Ok(new { Success = true, message = "Collaborator Added Successfully", data = collabRes });
                }
                else
                {
                    logger.LogError("Collab Creation Failed");
                    return BadRequest(new { Success = false, message = "The Email Doesn't Exists" });
                }
            }
            catch (AppException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Delete Request For Deleting A Collaborator (Delete: /api/collaborator/delete)
        /// </summary>
        /// <param name="collabId"></param>
        /// <param name="noteId"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        [HttpDelete("Delete")]
        public IActionResult DeleteCollaborator(long collabId, long noteId)
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                if (collabId <= 0)
                {
                    logger.LogWarning("Collab Id Should Be Greater Than Zero");
                    return BadRequest(new { success = false, message = "Collab Id Should Be Greater Than Zero" });
                }
                var collabRes = collabBL.DeleteCollaborator(collabId, noteId, userId);
                if (collabRes.Contains("Deleted"))
                {
                    logger.LogInformation(collabRes);
                    return Ok(new { Success = true, message = collabRes });
                }
                else
                {
                    logger.LogError(collabRes);
                    return Unauthorized(new { Success = false, message = collabRes });
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return Unauthorized(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get Request For Retrieving List Of Collaborator (Get: /api/collaborator/get)
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        [HttpGet("Get")]
        public IActionResult GetNoteCollaborators(long noteId)
        {
            try
            {
                if (noteId <= 0)
                {
                    logger.LogWarning("Note Id Should Be Greater Than Zero");
                    return BadRequest(new { success = false, message = "Note Id Should Be Greater Than Zero" });
                }
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var collabRes = collabBL.GetNoteCollaborators(noteId, userId);
                if (collabRes != null)
                {
                    logger.LogInformation("Got The Collaboraters Successfully");
                    return Ok(new { Success = true, message = "Got The Collaboraters Successfully", data = collabRes });
                }
                else
                {
                    logger.LogWarning("You Dont Have Access To Those Notes");
                    return Unauthorized(new { Success = false, message = "You Dont Have Access To Those Notes", data = collabRes });
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return Unauthorized(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get Request For Retrieving List Of Collabors (Get: /api/collaborator/get)
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult GetAllNoteCollabUsers()
        {
            try
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var collabRes = collabBL.GetAllNotesCollaborators();
                if (collabRes != null)
                {
                    logger.LogInformation("Got The Collaboraters Successfully");
                    return Ok(new { Success = true, message = "Got All The Collaboraters Successfully", data = collabRes });
                }
                else
                {
                    logger.LogWarning("No Collab Notes Present");
                    return NotFound(new { Success = false, message = "No Collab Notes Present", data = collabRes });
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get Request For Retrieving List Of Collaborator using redis(Get: /api/collaborator/redis)
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCollabUsingRedisCache(long noteId)
        {
            try
            {
                var cacheKey = "collabList";
                string serializedCollabNoteList;
                var collabsNoteList = new List<CollabListResponse>();
                var redisCollabsNoteList = await distributedCache.GetAsync(cacheKey);
                if (redisCollabsNoteList != null)
                {
                    logger.LogDebug("Getting The List From Redis Cache");
                    serializedCollabNoteList = Encoding.UTF8.GetString(redisCollabsNoteList);
                    collabsNoteList = JsonConvert.DeserializeObject<List<CollabListResponse>>(serializedCollabNoteList);
                }
                else
                {
                    logger.LogDebug("Setting The Collab List To Cache Which Request For First Time");
                    //Getting The Id Of Authorized User Using Claims Of Jwt
                    long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                    collabsNoteList = collabBL.GetNoteCollaborators(noteId, userId).ToList();
                    if (collabsNoteList != null)
                    {
                        serializedCollabNoteList = JsonConvert.SerializeObject(collabsNoteList);
                        redisCollabsNoteList = Encoding.UTF8.GetBytes(serializedCollabNoteList);
                        var options = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                        await distributedCache.SetAsync(cacheKey, redisCollabsNoteList, options);
                    }
                    else
                    {
                        return Unauthorized(new { success = false, message = "You Dont Have Access To Those Notes" });
                    }
                }
                logger.LogInformation("Got The Collaboraters Successfully Using Redis");
                return Ok(collabsNoteList);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return Unauthorized(new { success = false, message = ex.Message });
            }
        }
    }
}
