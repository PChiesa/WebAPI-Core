using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("Event")]
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }

        [ForeignKey("StoreId")]
        public Store Store { get; set; }
    }
}