using Proje1.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proje1.Models
{
    [Table("Saler")]
    public class Saler
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Password { get; set; }    
        public DateTime? Deleted { get; set; }
    }

    public class Login
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class SalerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Response Response { get; set; } = new Response();

    }
}
