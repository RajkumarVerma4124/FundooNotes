using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class LoginResponse
    {
        public UserEntity UserData { get; set; }
        public string Token { get; set; }
    }
}
