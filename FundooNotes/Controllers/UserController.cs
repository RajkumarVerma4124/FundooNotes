using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;
        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }

        [HttpPost("Register")]
        public IActionResult Post(UserReg userReg)
        {
            try
            {
                var res = userBL.Register(userReg);
                if (res != null)
                    return Ok(new { success = true, message = "Data Posted Successfully", data = res });
                else
                    return BadRequest(new { success = false, message = "Data Posted UnSuccessfully"});
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
    }
}
