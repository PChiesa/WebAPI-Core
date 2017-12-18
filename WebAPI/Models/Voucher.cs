using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Enums;

namespace WebAPI.Models
{
    [Table("Voucher")]
    public class Voucher
    {
        [Key]
        public int Id { get; set; }
        public int EventId { get; set; }
        public string ClientEventId { get; set; }
        public int UserId { get; set; }
        public string ClientUserId { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public VoucherStatus CurrentStatus { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }
}