using Proje1.Http;

namespace Proje1.ViewModel
{
    public class SalerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public DateTime? Deleted { get; set; }
        public Response? Response { get; set; }
    }
}
