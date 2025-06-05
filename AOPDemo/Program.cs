

namespace AOPDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();

            //builder.Services.AddOpenTelemetryTracing(builder =>
            //{
            //    builder
            //        .AddRougamoSource() // 初始化Rougamo.OpenTelemetry
            //        .AddAspNetCoreInstrumentation()
            //        .AddJaegerExporter();
            //});



            // 修改Rougamo.OpenTelemetry默认配置

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            // }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
