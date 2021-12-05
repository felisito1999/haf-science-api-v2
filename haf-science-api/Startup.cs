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
using FluentValidation;
using EmailService;
using EmailService.Models;
using EmailService.Services;
using EmailService.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using haf_science_api.Options;

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

            //FrontEndInfoConfig
            var frontEndAppConfig = Configuration.GetSection("FrontEndHafAppInfo")
                .Get<FrontEndHafAppInfo>();
            services.AddSingleton(frontEndAppConfig);

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

            //Email services
            var emailConfig = Configuration.GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.Configure<FormOptions>(opt =>
            {
                opt.ValueLengthLimit = int.MaxValue;
                opt.MultipartBodyLengthLimit = int.MaxValue;
                opt.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSenderService>();

            //Password services 
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IValidator<ChangePasswordModel>, PasswordValidator>();
            services.Configure<Options.PasswordOptions>(Configuration.GetSection("PasswordOptions"));

            //Controllers and data services
            services.AddScoped<IDataService<Estado, EstadosView>, EstadosService>();
            services.AddScoped<ICentrosEducativosService<CentrosEducativosModel, PaginatedCentrosEducativosView>, CentrosEducativosService>();
            services.AddScoped<ISessionService<SesionesModel, PaginatedSesionesView>, SesionesService>();
            services.AddScoped<IProvinciasService<Provincia>, ProvinciasService>();
            services.AddScoped<IMunicipiosService<Municipio>, MunicipiosService>();
            services.AddScoped<IUserHashesService<UserHash>, UsersHashesService>();
            services.AddScoped<IDataService<Role, RolView>, RolesService>();
            services.AddScoped<IPruebasDiagnosticasService<PruebasDiagnostica>, PruebasDiagnosticasService>();
            services.AddScoped<IPreguntasService<Pregunta>, PreguntasService>();

            //services.AddScoped<IDataService<CentrosEducativo>, CentrosEducativosService>();
            //services.AddScoped<IDataService<Estado>, EstadosService>();
            //services.AddScoped<IDataService<CentrosEducativo>, CentrosEducativosService>();
            //services.AddScoped<IDataService<Estado>, EstadosService>();
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
                    options.WithOrigins("http://localhost:3000", "http://10.0.0.6:3000", "https://haf-science-app-gxj3q.ondigitalocean.app");
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
