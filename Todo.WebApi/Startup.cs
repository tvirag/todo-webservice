using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todo.Data;
using Microsoft.EntityFrameworkCore;
using Todo.Interfaces;
using Todo.Model;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Service.Messaging;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using System.Buffers;

namespace Todo.WebApi
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
            // ********************
            // Setup CORS
            // ********************
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin(); // For anyone access.
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Todo API", Version = "v1" });
            });

            // Add framework services.
            services.AddDbContext<DataContext>(options =>
                //options.UseSqlite("Data Source=c:\\mydb.db;Version=3;")
                options.UseInMemoryDatabase("TodoDB")
                //options.UseSqlServer(Configuration.GetConnectionString("TodoDB"))
            );

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<DataContext>()
            //    .AddDefaultTokenProviders();

            services.AddMvc();
            services.Configure<MvcOptions>(options =>
            {
                var formatterSettings = JsonSerializerSettingsProvider.CreateSerializerSettings();
                options.OutputFormatters.Add(new JsonOutputFormatter(formatterSettings, ArrayPool<Char>.Shared));
            });
            services.AddScoped<IDataContext, DataContext>();
            services.AddScoped<IRepository<TodoItem>, Repository<TodoItem>>();
            services.AddScoped<MyFilterAttribute>();
            services.AddSingleton<MessageQueue>();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            

            app.UseSwagger();
            //app.UseMiddleware<ServiceMiddleware>();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.  
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
