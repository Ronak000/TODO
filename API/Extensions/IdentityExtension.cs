using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentityExtension(this IServiceCollection services,IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme
                ).AddJwtBearer(cfg =>
                {
                    //cfg.Authority = "http://localhost:5001";
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidAudience = config["Auth:Audience"],
                        ValidIssuer = config["Auth:Issuer"],
                       // ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]))
             
                    };
                });
                return services;
        }
    }
}