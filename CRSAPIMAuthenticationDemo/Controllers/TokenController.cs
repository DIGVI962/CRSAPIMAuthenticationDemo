using CRSAPIMAuthenticationDemo.Configuration;
using CRSAPIMAuthenticationDemo.Data;
using CRSAPIMAuthenticationDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace CRSAPIMAuthenticationDemo.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly AzureAdOptions _azureOptions;
        private readonly TokenDbContext _context;

        public TokenController(IOptions<AzureAdOptions> azureOptions, TokenDbContext context)
        {
            _azureOptions = azureOptions.Value;
            _context = context;
        }

        [HttpPost("generate")]
        public IActionResult GenerateEncryptedToken([FromBody] GenerateTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.MacAddress))
                return BadRequest("MAC address is required");

            var salt = "1836-2854-1090";
            var encryptedToken = EncryptToken(request.MacAddress, salt);

            var storedValue = _context.TokenRecords.FirstOrDefault(x => x.EncryptedToken == encryptedToken);
            if (storedValue != null)
            {
                return Ok(new { EncryptedToken = storedValue.EncryptedToken });
            }

            var tokenRecord = new TokenRecord
            {
                EncryptedToken = encryptedToken,
                CreatedAt = DateTime.UtcNow
            };

            _context.TokenRecords.Add(tokenRecord);
            _context.SaveChanges();

            return Ok(new { EncryptedToken = encryptedToken });
        }

        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenRequest request)
        {
            var tokenRecord = _context.TokenRecords
                .FirstOrDefault(x => x.EncryptedToken == request.EncryptedToken);

            if (tokenRecord == null)
                return Unauthorized("Please Generate an Identification Token first");

            try
            {
                var accessToken = await GetAzureOAuthToken();
                return Ok(new { AccessToken = accessToken });
            }
            catch
            {
                return Problem(statusCode: (int)HttpStatusCode.InternalServerError, detail: "There was a probelm generating OAuth Token");
            }
        }

        private string EncryptToken(string macAddress, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var rawData = macAddress + salt;
                var hashedData = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return Convert.ToBase64String(hashedData);
            }
        }

        private async Task<string> GetAzureOAuthToken()
        {
            var clientId = _azureOptions.ClientId;
            var clientSecret = _azureOptions.ClientSecrets;
            var tenantId = _azureOptions.TenantId;
            var scopes = _azureOptions.scopes;

            var cca = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
                .Build();

            var result = await cca.AcquireTokenForClient(scopes).ExecuteAsync();
            return result.AccessToken;
        }
    }
}
