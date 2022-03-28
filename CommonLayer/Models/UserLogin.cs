using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Class For User Login Request
    /// </summary>
    public class UserLogin
    {
        public string EmailId { get; set; }
        public string Password { get; set; }
    }
}
