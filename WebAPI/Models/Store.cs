using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("Store")]
    public class Store
    {
        [Key]
        public int Id { get; set; }

        public string ClientStoreId { get; set; }

        public string Name { get; set; }

        public string SecretKey { get; set; }

        public string Hash { get; set; }
    }
}