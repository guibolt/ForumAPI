using AutoMapper;
using Core.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace ApiForum
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
            services.AddDbContext<ForumContext>(options => options.UseSqlServer(Configuration.GetConnectionString("StringConexao")), ServiceLifetime.Scoped);


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info { Title = "Forum Api", Version = "v1" });
            });

            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.SerializerSettings.DateFormatString = "dd/MM/yyyy";
                opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });


            //Injeção de dependencia do automapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingConfig());

            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();

            // Ativa o Swagger UI
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiProject V1");
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiProject V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
