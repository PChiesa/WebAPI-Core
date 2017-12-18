using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("User")]
    public class User
    {
        public User()
        {
        }

        [Key]
        public int Id { get; set; }
        public string ClientUserId { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public string PasswordConfirmation { get; set; }

    }
}
