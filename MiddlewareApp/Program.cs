using Microsoft.AspNetCore.Rewrite;

namespace MiddlewareApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<CustomMiddleware>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseMiddleware<HS>();

            //var option = new RewriteOptions().AddRedirect("redirect-rule/(.*)", "redirected/$1");
            //app.UseRewriter(option);

            app.Map("/map1/seg1", HandleMapTest1);
            app.Map("/map2", HandleMapTest2);

            app.MapWhen(context => context.Request.Query.ContainsKey("mapbranch"), HandleBranch);

            app.MapWhen(context => context.Request.Headers.ContainsKey("mapbranch"), HandleBranch);

            app.UseWhen(context => context.Request.Query.ContainsKey("usebranch"), app => HandleBranchAndRejoin(app));

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("before1");
                await next.Invoke();
                await context.Response.WriteAsync("after1 <br/>");
            });

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("<br>before2 </br>");
                await next.Invoke();
                await context.Response.WriteAsync("after2 </br>");
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Run Reponse Write </br>");
            });

            app.Run();

            static void HandleMapTest1(IApplicationBuilder app)
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("Map Test 1");
                });
            }

            static void HandleMapTest2(IApplicationBuilder app)
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("Map Test 2");
                });
            }

            static void HandleBranch(IApplicationBuilder app)
            {
                app.Run(async context =>
                {
                    var branch = context.Request.Query["branch"];
                    await context.Response.WriteAsync($"map query branch value:{branch}<br/>");

                    var header = context.Request.Headers["branch"];
                    await context.Response.WriteAsync($"map header branch value:{header}<br/>");
                });
            }

            void HandleBranchAndRejoin(IApplicationBuilder app)
            {
                app.Use(async (context, next) =>
                {
                    var branchVer = context.Request.Query["usebranch"];

                    await context.Response.WriteAsync($"use query branch value:{branchVer}<br>");

                    // Do work that doesn't write to the Response.
                    await next();
                    // Do other work that doesn't write to the Response.
                });
            }
        }
    }
}
