using cookie_stand_api.Data;
using cookie_stand_api.Model;
using cookie_stand_api.Model.Interfaces;
using cookie_stand_api.Model.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace cookie_stand_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
                   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            builder.Services.AddControllers()
                   .AddJsonOptions(options =>
                   {
                       options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                   });

            string? connS = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<CookieSalmonDbContext>(options =>
            options.UseSqlServer(connS));


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost3010",
                    builder => builder.WithOrigins("http://localhost:3010",
                    "http://localhost:7001",
                    "http://localhost:7002",
                    "http://localhost:7003") 
                                     .AllowAnyHeader()
                                     .AllowAnyMethod());
            });


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<CookieSalmonDbContext>();


            builder.Services.AddTransient<ICookieStand, CookieStandService>();

            builder.Services.AddTransient<IUser, ApplicationUserService>();

            builder.Services.AddScoped<JWTTokenService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {

                options.TokenValidationParameters = JWTTokenService.GetValidationPerameters(builder.Configuration);
            });


            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Create", policy => policy.RequireClaim("permissions", "Create"));
                options.AddPolicy("Update", policy => policy.RequireClaim("permissions", "Update"));
                options.AddPolicy("Delete", policy => policy.RequireClaim("permissions", "Delete"));
                options.AddPolicy("Read", policy => policy.RequireClaim("permissions", "Read"));
            });

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Cookie Salmon API",
                    Version = "v1",
                });
            });

            var app = builder.Build();
            app.UseCors("AllowLocalhost3000");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "/api/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/api/v1/swagger.json", "Cookie Salmon API");
                options.RoutePrefix = "";
            });

            app.MapControllers();


            app.Run();
        }
    }
}
