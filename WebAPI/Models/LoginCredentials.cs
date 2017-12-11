using System;
namespace WebAPI.Models
{
    public class LoginCredentials
    {
        public LoginCredentials()
        {
        }

        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
