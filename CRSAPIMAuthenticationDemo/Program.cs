using CRSAPIMAuthenticationDemo.Configuration;
using CRSAPIMAuthenticationDemo.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CRSAPIMAuthenticationDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<AzureAdOptions>(builder.Configuration.GetSection("AzureAd"));

            var azureAdOptions = builder.Configuration.GetSection("AzureAd").Get<AzureAdOptions>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //options.Authority = $"{azureAdOptions.Instance}{azureAdOptions.TenantId}";
                    options.Authority = $"https://login.microsoftonline.com/{azureAdOptions.TenantId}";
                    //options.Audience = azureAdOptions.ClientId;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = azureAdOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = azureAdOptions.Audience,
                        ValidateLifetime = true
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<TokenDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultSQLiteConnection")));

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

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
