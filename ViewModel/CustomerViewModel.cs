using Proje1.Http;
using Proje1.ViewModel;

namespace Proje1.ViewModel
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ProductId { get; set; }
        public int SalerId { get; set; }
        public Response? Response { get; set; }
    }

}
