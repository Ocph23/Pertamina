using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Data
{
    public class UserLogin
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }


    public class ChangePassword
    {

        public string UserId { get; set; }
        public string UserName{ get; set; }
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}