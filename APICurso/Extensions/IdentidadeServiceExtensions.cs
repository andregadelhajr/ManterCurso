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
        public static IServiceCollection AddIdentidadeServices(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<Usuario>();

            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddEntityFrameworkStores<UsuarioDbContext>();
            builder.AddSignInManager<SignInManager<Usuario>>();
            
            services.AddAuthentication();

            return services;
        }
    }
}


