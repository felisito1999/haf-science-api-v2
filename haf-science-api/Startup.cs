using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using haf_science_api.Services;
using AutoMapper;

namespace haf_science_api
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
            services.AddControllers();
            //Database services
            var connection = Configuration.GetConnectionString("HafScienceDatabase");
            
            services.AddDbContextPool<HafScienceDbContext>(
                options => options.UseSqlServer(connection));

            //Authentication services
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            //Password services 
            services.AddScoped<IPasswordService, PasswordService>();
            services.Configure<Options.PasswordOptions>(Configuration.GetSection("PasswordOptions"));

            //Controllers and data services
            services.AddScoped<IDataService<Estado, EstadosView>, EstadosService>();
            services.AddScoped<IDataService<CentrosEducativosModel, PaginatedCentrosEducativosView>, CentrosEducativosService>();
            services.AddScoped<IDataService<Role, RolView>, RolesService>();
            //services.AddScoped<IDataService<CentrosEducativo>, CentrosEducativosService>();
            //services.AddScoped<IDataService<Estado>, EstadosService>();
            //services.AddScoped<IDataService<CentrosEducativo>, CentrosEducativosService>();
            //services.AddScoped<IDataService<Estado>, EstadosService>();
            //services.AddScoped<IDataService<CentrosEducativo>, CentrosEducativosService>();
            //services.AddScoped<IDataService<Estado>, EstadosService>();
            //services.AddScoped<IDataService<CentrosEducativo>, CentrosEducativosService>();
            //services.AddScoped<IDataService<Estado>, EstadosService>();
            //services.AddScoped<IDataService<CentrosEducativo>, CentrosEducativosService>();
            //services.AddScoped<IDataService<Estado>, EstadosService>();
            //services.AddScoped<IDataService<CentrosEducativo>, CentrosEducativosService>();
            //services.AddScoped<IDataService<Estado>, EstadosService>();
            //services.AddScoped<IDataService<CentrosEducativo>, CentrosEducativosService>();
            //services.AddScoped<IDataService<Estado>, EstadosService>();
            //services.AddScoped<IDataService<CentrosEducativo>, CentrosEducativosService>();
            //services.AddScoped<IDataService<Estado>, EstadosService>();
            //services.AddScoped<IDataService<CentrosEducativo>, CentrosEducativosService>();

            services.AddScoped<IUserService<UsuariosModel>, UsuariosService>();
            
            services.AddScoped<ITokenService, TokenService>();

            //Mapper configuration
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            //Swagger configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "haf_science_api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(
                options =>
                {
                    options.WithOrigins("http://localhost:3000", "http://10.0.0.6:3000");
                    options.AllowAnyMethod();
                    options.AllowAnyHeader();
                });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "haf_science_api v1"));
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
