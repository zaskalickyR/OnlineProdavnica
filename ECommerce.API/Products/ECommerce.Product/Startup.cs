using AutoMapper;
using ECommerce.DAL.Data;
using ECommerce.DAL.Mappings;
using ECommerce.DAL.Services.Implementations;
using ECommerce.DAL.Services.Interfaces;
using ECommerce.DAL.UOWs;
using MailKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ECommerce.DAL.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerce.Product
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

            services.AddAuthentication();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web2eCommerce", Version = "v1" });
            });


            //services.AddDbContext<ProductDbContext>(options => options.UseSqlServer("Server=localhost;Database=ECommerce_Products;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True;"));
            services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(Configuration["dataBaseConnectionString"]));

            //Registracija mapera u kontejneru, zivotni vek singleton
            var mapperConfig = new MapperConfiguration(mc =>
            {
                //mc.AddProfile(new UserProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            var provider = services.BuildServiceProvider();
            // var configure

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "Issuer",
                    ValidAudience = "Audience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("b90ajAJiosjdASF93261a4d351e7gasd(0k0daj@Qjcf478ea8d312c763bb6caca")),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });
            services.AddAuthorization();



            MappingServices(services);
            BindServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "eCommerce"));                  // 
            }

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseRouting();


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors("AllowAll");
            });
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "images")), // Putanja do foldera images
                RequestPath = "/images", // Prefiks URL-a za pristup folderu images
                EnableDirectoryBrowsing = true // Dozvoljava pregled direktorijuma
            });

        }

        private void BindServices(IServiceCollection services)
        {
            services.AddDbContext<ProductDbContext>();
            services.AddHttpClient();
            services.AddTransient<IHttpClientService, HttpClientService>();
            services.AddTransient<IUnitOfWorkProduct, UnitOfWorkProduct>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        };
                    });

        }
        private void MappingServices(IServiceCollection services)
        {
            //bind mappings
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(CategoryProfile));
        }
    }
}
