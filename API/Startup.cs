using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;

namespace GUFOS
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
            services.AddControllersWithViews().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

             services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  
            .AddJwtBearer(options =>  
            {  
                options.TokenValidationParameters = new TokenValidationParameters  
                {  
                    ValidateIssuer = true, // Habilita a validação do Issuer do Token
                    ValidateAudience = true, // Habilita a validação do Audience do Token
                    ValidateLifetime = true, // Habilita a validação do tempo de expiração do Token
                    ValidateIssuerSigningKey = true, //Permite que a Microsofot valide nosso token

                    ValidIssuer = Configuration["Jwt:Issuer"],// Define um valor válido para um Issuer
                    ValidAudience = Configuration["Jwt:Issuer"], //Define um valor válido para o Audience
                    // A linha abaixo: Issuer Signing Key será igual ao resultado do SymmetricSecurityKey recebendo a Jwt:Key criptografada
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])) 

                };  
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                // Mostrar o caminho dos comentários dos métodos Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            // services.AddControllersWithViews().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore); 
            // colar isso aqui no lugar de services.AddControllers();/ usado para ignorar o loop
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // Habilitamos efetivamente o Swagger em nossa aplicação.
            app.UseSwagger();
            // Especificamos o endpoint da documentação
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
        }
    }
}
