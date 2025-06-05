using Minio;

namespace MinIO.Lesson
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var endpoint = "192.168.6.216";
            var accessKey = "MkMjY6GXoFgAxpPYIUhi";
            var secretKey = "it35D6tHcEgYaHJGMPEDDV2V8pRqSXvvnivpfx8P";

            var builder = WebApplication.CreateBuilder(args);
            // builder.Services.AddMinio(accessKey, secretKey);

            builder.Services.AddMinio(configureClient => configureClient.WithEndpoint(endpoint, 7000).WithCredentials(accessKey, secretKey));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
