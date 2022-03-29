using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Entity
{
    /// <summary>
    /// Created The Class For Login Response 👈 
    /// </summary>
    public class LoginResponse
    {
        public UserEntity UserData { get; set; }
        public string Token { get; set; }
    }
}
