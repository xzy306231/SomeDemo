using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Minio;
using MinIoDemo.IService;
using MinIoDemo.Model;
using MinIoDemo.Service;

namespace MinIoDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMinio(minioClient => minioClient.WithEndpoint(Configuration["MinioSettings:Endpoint"]).WithCredentials(Configuration["MinioSettings:AccessKey"],
                                           Configuration["MinioSettings:SecretKey"]));


            services.AddScoped<IMinIOService, MinIOService>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MinIoDemo", Version = "v1" });
                var file = "C:\\Users\\admin\\Desktop\\Demos\\MinIoDemo\\bin\\Debug\\net5.0\\MinIoDemo.xml";
                c.IncludeXmlComments(file);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MinIoDemo v1"));


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
