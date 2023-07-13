
using Proje1.Http;

namespace Proje1.ViewModel
{
    public class SalesProductsViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int SalesCount { get; set; }
        public Response? Response { get; set; }
    }
}
