using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FundooNotes.Controllers
{
    /// <summary>
    /// Created The User Controller For Http Requests And Response 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Object Reference For Interface IUserBL
        private readonly IUserBL userBL;

        //Constructor To Initialize The Instance Of Interface IUserBL
        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }

        //Post Request For Registering A New User (POST: /api/user/register)
        [HttpPost("Register")]
        public IActionResult Register(UserReg userReg)
        {
            try
            {
                var resUser = userBL.Register(userReg);
                if (resUser != null)
                    return Ok(new { success = true, message = "Registeration Successfully", data = resUser });
                else
                    return BadRequest(new { success = false, message = "Registeration Failed EmailId Already Exist"});
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Post Request For Login Existing User (POST: /api/user/login)
        [HttpPost("Login")]
        public IActionResult Login(UserLogin userLogin)
        {
            try
            {
                var resUser = userBL.Login(userLogin);
                if (resUser != null)
                    return Ok(new { success = true, message = "Login Successfully", data = resUser});
                else
                    return BadRequest(new { success = false, message = "Login Failed Check EmailId And Password" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
