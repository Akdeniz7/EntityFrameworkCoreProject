using Proje1.Models;

namespace Proje1.FormModel
{
    public class SalesProductFormModel
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public int SalesCount { get; set; }
    }

    public class MultipleProduct
    {
        public List<SalesProducts> multProduct { get; set; } = new List<SalesProducts>();
    }
}
