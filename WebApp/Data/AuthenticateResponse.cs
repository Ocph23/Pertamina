using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Data
{
    // public class User
    // {
    //     public int Id { get; set; }
    //     public string FirstName { get; set; }
    //     public string LastName { get; set; }
    //     public string Username { get; set; }

    //     [JsonIgnore]
    //     public string Password { get; set; }
    // }

    public class AuthenticateResponse
    {
        public string Token { get; set; }


        public AuthenticateResponse(IdentityUser user, string token)
        {
            Token = token;
        }
    }
}