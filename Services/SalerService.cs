using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Proje1.DBContext;
using Proje1.FormModel;
using Proje1.Http;
using Proje1.Models;
using Proje1.ViewModel;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Proje1.Services
{
    public class SalerService
    {
        private readonly SalesDBContext context;
        private readonly IConfiguration config;
        private readonly SalerDto profile;

        public SalerService(SalesDBContext _context, IConfiguration _config, SalerDto _profile)
        {
            context = _context;
            config = _config;
            profile = _profile;
        }

        public SalerViewModel Login(Login model)
        {
            var data = (from k in context.Saler
                        where k.Deleted == null && k.Name == model.Name && k.Password == model.Password
                        select new SalerViewModel
                        {
                            Id = k.Id,
                            Name = k.Name,
                            Password = k.Password,

                            Response = new Response
                            {
                                StatusCode = 200,
                                Success = true,
                                Message = "Kullanıcı Girişi Başarılı",
                            }
                        }).FirstOrDefault();
            if(data != null)
            {
                var claims = new[]
                {
                    new Claim("Id", data.Id.ToString()),
                    new Claim("Name", data.Name),
                    new Claim("Password", data.Password)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                        config["Jwt:Issuer"],
                        config["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddDays(30),
                        signingCredentials: signIn);

                data.Token = new JwtSecurityTokenHandler().WriteToken(token);
            }
            return data;
        }

        public List<SalerViewModel> getSalerProfile()
        {
            var data = (from k in context.Saler
                        where k.Deleted == null && k.Id == profile.Id
                        select new SalerViewModel
                        {
                            Id = k.Id,
                            Name = k.Name,
                            Password = k.Password,

                        });

            return data.ToList();
        }

        public List<SalerViewModel> getSaler()
        {
            var data = (from k in context.Saler
                        where k.Deleted == null
                        select new SalerViewModel
                        {
                            Id = k.Id,
                            Name = k.Name,
                            Password = k.Password,

                        });

            return data.ToList();
        }

        public SalerDto ResolveUserToken(string accessToken, string source="")
        {
            try
            {
                return ResolveUserToken(accessToken);
            }
            catch (Exception ex)
            {
                return new SalerDto
                {
                    Response = new Response
                    {
                        StatusCode = 401,
                        Success = false,
                        Message = "Invalid token"
                    }
                };
            }
        }

        public SalerDto ResolveUserToken(string token)
        {
            var jwtConfig = this.config.GetSection("Jwt");
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig["Key"]));

            var tokenValidationParamters = new TokenValidationParameters
            {
                ValidateAudience = false, 
                ValidateIssuer = false,
                ValidateActor = false,
                ValidateLifetime = false,
                IssuerSigningKey = signingKey
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParamters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token!!!");
            }

            int ID = Convert.ToInt32(principal.FindFirst("Id")?.Value);
            string Name = principal.FindFirst("Name")?.Value;
            string Password = principal.FindFirst("Password")?.Value;
            Response response = new Response
            {
                StatusCode = 200,
                Success = true,
                Message = "Kullanıcı Girişi Başarılı"
            };

            var personel = new SalerDto
            {
                Id = ID,
                Name = Name,
                Password = Password,
                Response = response
            };

            return personel;
        }

        public async Task<Response> InsertSalerAsync(SalerFormModel model)
        {
            Response response = new Response();
            try
            {
                if (model.ID == 0)
                {
                    Saler saler = new Saler();
                    saler.Name = model.Name;
                    saler.Password = model.Password;
                    context.Add(saler);

                    await context.SaveChangesAsync();

                    response = new Response
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = "User successfully added to database"
                    };
                }
                else
                {
                    var user = (from k in context.Saler where k.Id == model.ID select k).FirstOrDefault();


                    user.Name = model.Name;
                    user.Password = model.Password; 

                    await context.SaveChangesAsync();

                    response = new Response
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = "User successfully uptaded"
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

        public async Task<Response> InsertMultipleSalerAsync(MultipleSaler model)
        {
            Response response = new Response();
            try
            {   if(model.multSaler.Count > 0)
                {
                    for (int i = 0; i < model.multSaler.Count; i++)
                    {
                        var sa = model.multSaler[i];
                        if (sa.Id==0)
                        {
                            Saler saler = new Saler();

                            saler.Name = sa.Name;
                            saler.Password = sa.Password;

                            context.Add(saler);

                            await context.SaveChangesAsync();

                            response = new Response
                            {
                                Success = true,
                                StatusCode = 200,
                                Message = "Salers successfully added to database"
                            };
                        }
                        else
                        {
                            var data = (from m in context.Saler where m.Id == model.multSaler[i].Id select m).FirstOrDefault();

                            data.Name = model.multSaler[i].Name;
                            data.Password = model.multSaler[i].Password;

                            await context.SaveChangesAsync();

                            response = new Response
                            {
                                Success = true,
                                StatusCode = 200,
                                Message = "Salers successfully uptaded"
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

        public async Task<Response> DeleteMultipleSalerAsync(MultipleSaler model)
        {
            Response response = new Response();
            try
            {
                if (model.multSaler.Count > 0)
                {
                    for (int i = 0; i < model.multSaler.Count; i++)
                    {
                        var sa = model.multSaler[i];
                        var data = (from m in context.Saler where m.Id == model.multSaler[i].Id select m).FirstOrDefault();

                        data.Deleted = DateTime.Now;

                        await context.SaveChangesAsync();

                        response = new Response
                        {
                            Success = true,
                            StatusCode = 200,
                            Message = "Salers successfully deleted"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                response = new Response
                {
                    Success = false,
                    StatusCode = 422,
                    Message = "Salers could not be deleted",
                    Exception = ex.Message
                };
            }
            return response;
        }


        public async Task<Response> DeleteSalerAsync(SalerFormModel model)
        {
            Response response = new Response();

            try
            {
                var user = (from k in context.Saler where k.Id == model.ID select k).FirstOrDefault();

                user.Deleted = DateTime.Now;

                await context.SaveChangesAsync();

                response = new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "User successfully deleted"
                };
            }
            catch (Exception ex)
            {
                response = new Response
                {
                    Success = false,
                    StatusCode = 422,
                    Message = "User could not be deleted",
                    Exception = ex.Message
                };

            }

            return response;
        }
    }
}
