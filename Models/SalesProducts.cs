using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proje1.Models
{
    [Table("SalesProducts")]
    public class SalesProducts
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int SalesCount { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
