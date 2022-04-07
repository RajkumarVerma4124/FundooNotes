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

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollabController : ControllerBase
    {
        //Reference Object For Interface IUserRL,IDistributedCache,IMemoryCache
        private readonly ICollabBL collabBL;
        private readonly IDistributedCache distributedCache;
        private readonly IMemoryCache memoryCache;

        //Created Constructor With Dependency Injection For IUSerRL
        public CollabController(ICollabBL collabBL, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.collabBL = collabBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
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

        [HttpGet("redis")]
        public async Task<IActionResult> GetAllLabelUsingRedisCache(long noteId)
        {
            var cacheKey = "collabList";
            string serializedCollabNoteList;
            var collabsNoteList = new List<CollaboratorEntity>();
            var redisCollabsNoteList = await distributedCache.GetAsync(cacheKey);
            if (redisCollabsNoteList != null)
            {
                serializedCollabNoteList = Encoding.UTF8.GetString(redisCollabsNoteList);
                collabsNoteList = JsonConvert.DeserializeObject<List<CollaboratorEntity>>(serializedCollabNoteList);
            }
            else
            {
                //Getting The Id Of Authorized User Using Claims Of Jwt
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                collabsNoteList = collabBL.GetNoteCollaborators(noteId, userId).ToList();
                serializedCollabNoteList = JsonConvert.SerializeObject(collabsNoteList);
                redisCollabsNoteList = Encoding.UTF8.GetBytes(serializedCollabNoteList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisCollabsNoteList, options);
            }
            return Ok(collabsNoteList);
        }
    }
}
