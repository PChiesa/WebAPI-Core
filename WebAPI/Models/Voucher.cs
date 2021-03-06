using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Enums;

namespace WebAPI.Models
{
    public class Voucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EventId { get; set; }
        public string ClientCPFOwner { get; set; }
        public string ClientNameOwner { get; set; }
        public string ClientOrderId { get; set; }
        public string ClientTicketId { get; set; }
        public string ClientEventId { get; set; }
        public int UserId { get; set; }
        public string ClientUserId { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
        public string Token { get; set; }
        public string Gate { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime FinishedDate { get; set; }
        public VoucherStatus CurrentStatus { get; set; }

        [JsonIgnore]
        [ForeignKey("EventId")]
        public Event Event { get; set; }

        [NotMapped]
        public string ClientUserEmail { get; set; }

        [NotMapped]
        public string ClientUserCpf { get; set; }
    }
}