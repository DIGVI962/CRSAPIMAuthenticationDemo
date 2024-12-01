using CRSAPIMAuthenticationDemo.Configuration;
using CRSAPIMAuthenticationDemo.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

namespace CRSAPIMAuthenticationDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<AzureAdOptions>(builder.Configuration.GetSection("AzureAd"));
            builder.Services.Configure<AzureAdClientOptions>(builder.Configuration.GetSection("AzureAdClient"));

            var azureAdClientOptions = builder.Configuration.GetSection("AzureAdClient").Get<AzureAdClientOptions>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration);

            builder.Services.AddDbContext<TokenDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultSQLiteConnection")));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
