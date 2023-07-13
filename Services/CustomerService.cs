using Proje1.DBContext;
using Proje1.FormModel;
using Proje1.Http;
using Proje1.Models;
using Proje1.ViewModel;
using System.Text.RegularExpressions;

namespace Proje1.Services
{
    public class CustomerService
    {
        private readonly SalesDBContext context;
        private readonly IConfiguration config;

        public CustomerService(SalesDBContext _context, IConfiguration _config)
        {
            context = _context;
            config = _config;
        }

        public List<CustomerViewModel> getCustomer()
        {
            var data = (from k in context.Customer
                        join p in context.Saler on k.SalerId equals p.Id into p1
                        from p in p1.DefaultIfEmpty()
                        join r in context.SalesProducts on k.ProductId equals r.Id into r1
                        from r in r1.DefaultIfEmpty()

                        where k.Deleted == null
                        orderby k.SalerId
                        select new CustomerViewModel
                        {
                            CustomerId = k.CustomerId,
                            CustomerName = k.CustomerName,
                            ProductId = r.Id,
                            SalerId = p.Id,

                        }); 

            return data.ToList(); 
        }


        public async Task<Response> InsertCustomer(CustomerFormModel model)
        {
            Response response = new Response();
            try
            {
                if (model.CustomerId == 0)
                {
                    Customer customer = new Customer();
                    customer.CustomerName = model.CustomerName;
                    customer.ProductId = model.ProductId;
                    customer.SalerId = model.SalerId;
                    context.Add(customer);

                    await context.SaveChangesAsync();

                    response = new Response
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = "New customer successfully added to database"
                    };
                }
                else
                {
                    var user = (from k in context.Customer where k.CustomerId == model.CustomerId select k).FirstOrDefault();


                    user.CustomerName = model.CustomerName;

                    await context.SaveChangesAsync();

                    response = new Response
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = "Customer successfully uptaded"
                    };

                }
            }
            catch (Exception ex)
            {
                response = new Response
                {
                    Success = false,
                    StatusCode = 422,
                    Message = ex.Message
                };
            }

            return response;
        }

        public async Task<Response> InsertMultipleCustomerAsync(MultipleCustomer model)
        {
            Response response = new Response();
            try
            {
                if (model.multCustomer.Count > 0)
                {
                    for (int i = 0; i < model.multCustomer.Count; i++)
                    {
                        var sa = model.multCustomer[i];
                        if (sa.CustomerId == 0)
                        {
                            Customer customer = new Customer();
                            customer.CustomerName = sa.CustomerName;
                            customer.ProductId = sa.ProductId;
                            customer.SalerId = sa.SalerId;
                            context.Add(customer);

                            await context.SaveChangesAsync();

                            response = new Response
                            {
                                Success = true,
                                StatusCode = 200,
                                Message = "Customers successfully added to database"
                            };
                        }
                        else
                        {
                            var data = (from m in context.Customer where m.CustomerId == model.multCustomer[i].CustomerId select m).FirstOrDefault();

                            data.CustomerName = model.multCustomer[i].CustomerName;
                            data.ProductId = model.multCustomer[i].ProductId;
                            data.SalerId = model.multCustomer[i].SalerId;

                            await context.SaveChangesAsync();

                            response = new Response
                            {
                                Success = true,
                                StatusCode = 200,
                                Message = "Customers successfully uptaded"
                            };
                        }
                    }
                }
                else
                {
                    response = new Response
                    {
                        Success = true,
                        StatusCode = 401,
                        Message = "Empty records"
                    };
                }
            }
            catch (Exception ex)
            {
                response = new Response
                {
                    Success = false,
                    StatusCode = 422,
                    Message = ex.Message
                };
            }
            return response;
        }


        public async Task<Response> DeleteCustomerAsync(CustomerFormModel model)
        {
            Response response = new Response();

            try
            {
                var user = (from k in context.Customer where k.CustomerId == model.CustomerId select k).FirstOrDefault();

                user.Deleted = DateTime.Now;

                await context.SaveChangesAsync();

                response = new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Customer successfully deleted"
                };
            }
            catch (Exception ex)
            {
                response = new Response
                {
                    Success = false,
                    StatusCode = 422,
                    Message = "Customer could not be deleted",
                    Exception = ex.Message
                };

            }

            return response;
        }
    }
}
