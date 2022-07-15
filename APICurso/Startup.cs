using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICurso.Extensions;
using APICurso.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace APICurso
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CursoContext>(
                contexto=> contexto.UseSqlServer(Configuration.GetConnectionString("ConexaoBDServer"))
            );

            services.AddDbContext<UsuarioDbContext>(
                contexto=> contexto.UseSqlServer(Configuration.GetConnectionString("UsuarioConexao"))
            );

            // services.AddDbContext<UsuarioDbContext>(
            //     contexto=> contexto.UseSqlite(Configuration.GetConnectionString("UsuarioConexao2"))
            // );

            // services.AddDbContext<CursoContext>(
            //     contexto=> contexto.UseSqlite(Configuration.GetConnectionString("ConexaoBDSlite"))
            // );

            services.AddControllers();
            services.AddIdentidadeServices();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "APICurso", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APICurso v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors(x=>x.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
