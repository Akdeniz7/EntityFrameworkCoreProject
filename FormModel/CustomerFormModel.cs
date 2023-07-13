using Proje1.Models;

namespace Proje1.FormModel
{
    public class CustomerFormModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ProductId { get; set; }
        public int SalerId { get; set; }
    }

    public class MultipleCustomer
    {
        public List<Customer> multCustomer { get; set; } = new List<Customer>();
    }
}
