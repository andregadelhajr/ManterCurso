using System.Text;
using APICurso.Models;
using APICurso.Models.Identidade;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace APICurso.Extensions
{
    public static class IdentidadeServiceExtensions
    {
        public static IServiceCollection AddIdentidadeServices(this IServiceCollection services, IConfiguration config)
        {
            var builder = services.AddIdentityCore<Usuario>();

            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddEntityFrameworkStores<UsuarioDbContext>();
            builder.AddSignInManager<SignInManager<Usuario>>();
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                    ValidIssuer = config["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false
                };
            });

            return services;
        }
    }
}


