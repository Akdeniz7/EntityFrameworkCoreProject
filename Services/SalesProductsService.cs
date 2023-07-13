using Proje1.DBContext;
using Proje1.FormModel;
using Proje1.Http;
using Proje1.Models;
using Proje1.ViewModel;

namespace Proje1.Services
{
    public class SalesProductsService
    {
        private readonly SalesDBContext context;
        private readonly IConfiguration config;

        public SalesProductsService(SalesDBContext _context, IConfiguration _config)
        {
            context = _context;
            config = _config;
        }

        public List<SalesProductsViewModel> getSalesProducts()
        {
            var data = (from k in context.SalesProducts
                        where k.Deleted == null
                        select new SalesProductsViewModel
                        {
                            Id = k.Id,
                            ProductName = k.ProductName,
                            SalesCount = k.SalesCount,

                        });

            return data.ToList();
        }

        public async Task<Response> InsertProductAsync(SalesProductFormModel model)
        {
            Response response = new Response();
            try
            {
                if (model.ID == 0)
                {
                    SalesProducts product = new SalesProducts();
                    product.ProductName = model.ProductName;
                    product.SalesCount = model.SalesCount;
                    context.Add(product);

                    await context.SaveChangesAsync();

                    response = new Response
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = "New product successfully added to database"
                    };
                }
                else
                {
                    var data = (from k in context.SalesProducts where k.Id == model.ID select k).FirstOrDefault();


                    data.ProductName = model.ProductName;
                    data.SalesCount = model.SalesCount;

                    await context.SaveChangesAsync();

                    response = new Response
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = "Product successfully uptaded"
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

        public async Task<Response> InsertMultipleProductAsync(MultipleProduct model)
        {
            Response response = new Response();
            try
            {
                if(model.multProduct.Count >0)
                {
                    for(int i=0; i<model.multProduct.Count; i++)
                    {
                        var sa = model.multProduct[i];
                        if(sa.Id == 0)
                        {
                            SalesProducts product = new SalesProducts();
                            product.ProductName = sa.ProductName;
                            product.SalesCount = sa.SalesCount;
                            context.Add(product);

                            await context.SaveChangesAsync();

                            response = new Response
                            {
                                Success = true,
                                StatusCode = 200,
                                Message = "Products successfully added to database"
                            };
                        }
                        else
                        {
                            var data = (from m in context.SalesProducts where m.Id == model.multProduct[i].Id select m).FirstOrDefault();

                            data.ProductName = model.multProduct[i].ProductName;
                            data.SalesCount = model.multProduct[i].SalesCount;

                            await context.SaveChangesAsync();

                            response = new Response
                            {
                                Success = true,
                                StatusCode = 200,
                                Message = "Products successfully uptaded"
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
            catch(Exception ex)
            {
                response = new Response
                {
                    Success = false,
                    StatusCode=422,
                    Message = ex.Message
                };
            }
            return response;
        }

        public async Task<Response> DeleteProductAsync(SalesProductFormModel model)
        {
            Response response = new Response();

            try
            {
                var user = (from k in context.SalesProducts where k.Id == model.ID select k).FirstOrDefault();

                user.Deleted = DateTime.Now;

                await context.SaveChangesAsync();

                response = new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Product successfully deleted"
                };
            }
            catch (Exception ex)
            {
                response = new Response
                {
                    Success = false,
                    StatusCode = 422,
                    Message = "Product could not be deleted",
                    Exception = ex.Message
                };

            }

            return response;
        }
    }
}
