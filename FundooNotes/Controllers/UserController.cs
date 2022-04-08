using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FundooNotes.Controllers
{
    /// <summary>
    /// Created The User Controller For Http Requests And Response 👈 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Object Reference For Interface IUserBL
        private readonly IUserBL userBL;
        private readonly ILogger<UserController> logger;

        //Constructor To Initialize The Instance Of Interface IUserBL
        public UserController(IUserBL userBL, ILogger<UserController> logger)
        {
            this.userBL = userBL;
            this.logger = logger;
        }

        //Post Request For Registering A New User (POST: /api/user/register)

        [HttpPost("Register")]
        public IActionResult Register(UserReg userReg)
        {
            try
            {
                var ifEmailExist = userBL.IsEmailIdExist(userReg.EmailId);
                if (ifEmailExist)
                {
                    logger.LogWarning("The EmailId Already Exist");
                    return Ok(new { success = false, message = "The EmailId Already Exist" });
                }
                var resUser = userBL.Register(userReg);
                if (resUser != null)
                {
                    logger.LogInformation("Registeration Successfull");
                    return Created("User Added Successfully", new { success = true, data = resUser });
                }
                else
                {
                    logger.LogError("Registeration Unsuccessfull");
                    return BadRequest(new { success = false, message = "Something Went Wrong" });
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
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
                {
                    logger.LogInformation("Login Successfull");
                    return Ok(new { success = true, message = "Login Successfully", Email = resUser.UserData.EmailId, token = resUser.Token });
                }
                else
                {
                    logger.LogError("Login Failed");
                    return BadRequest(new { success = false, message = "Login Failed Check EmailId And Password" });
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Post Request For Forgot Password Existing User (POST: /api/user/forgotpassword)
        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(string emailId)
        {
            try
            {
                var resUser = userBL.ForgetPassword(emailId);
                if (resUser != null)
                {
                    logger.LogInformation("Reset Link Sent Successfull");
                    return Ok(new { success = true, message = "Reset Link Sent Successfully" });
                }
                else
                {
                    logger.LogError("Something Went Wrong");
                    return BadRequest(new { success = false, message = "Unsuccessfull" });
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        //Patch Request For Resetting Password For Existing User (PUT: /api/user/resetpassword)
        [HttpPatch("ResetPassword")]
        [Authorize]  //👈 For Authorized User Only
        public IActionResult ResetPassword(ResetPassword resetPassword)
        {
            try
            {
                //Getting THe Email Of Authorized User Using Claims Of Jwt
                var emailId = User.FindFirst(ClaimTypes.Email).Value;
                var resMessage = userBL.ResetPassword(resetPassword, emailId);
                if (!resMessage.Contains("Not"))
                {
                    logger.LogInformation(resMessage);
                    return Ok(new { success = true, message = resMessage });
                }
                else
                {
                    logger.LogError(resMessage);
                    return BadRequest(new { success = false, message = resMessage });
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " Exception Thrown...");
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
