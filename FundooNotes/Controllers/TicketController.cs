using BusinessLayer.Interface;
using CommonLayer.Models;
using FundooNotes.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        /// <summary>
        /// Object Reference For Interface IUserBL
        /// </summary>
        private readonly IBus _bus;
        private readonly IUserBL userBL;

        /// <summary>
        /// Created Constructor To Initialize Bus And userRL For Each Instance
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="userBL"></param>
        public TicketController(IBus bus, IUserBL userBL)
        {
            this._bus = bus;
            this.userBL = userBL;
        }

        /// <summary>
        /// Method To Create Ticket For Consumer By Producer For Forgetting Password
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> CreateTicketForPassword(string emailId)
        {
            try
            {
                if (emailId != null)
                {
                    var token = userBL.ForgetPassword(emailId);
                    if (!string.IsNullOrEmpty(token))
                    {
                        var ticketResonse = userBL.CreateTicketForPassword(emailId, token);
                        Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
                        var endPoint = await _bus.GetSendEndpoint(uri);
                        await endPoint.Send(ticketResonse);
                        return Ok(new { success = true, message = "Email Sent Successfully" });
                    }
                    else
                    {
                        throw new KeyNotFoundException("Email Id Is Not Registered");
                    }
                }
                else
                {
                    throw new AppException("Something Went Wrong");
                }
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (AppException ex)
            {
                throw new AppException(ex.Message);
            }
        }
    }
}
