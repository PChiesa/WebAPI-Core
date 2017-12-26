using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("Stores")]
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ClientStoreId { get; set; }

        public string Name { get; set; }

        public string SecretKey { get; set; }

        public string Hash { get; set; }
    }
}