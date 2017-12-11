using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("Voucher")]
    public class Voucher
    {
        [Key]
        public int Id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
        public string Token { get; set; }
    }
}