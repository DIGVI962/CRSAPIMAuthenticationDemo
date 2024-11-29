using CRSAPIMAuthenticationDemo.Configuration;
using CRSAPIMAuthenticationDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Security.Cryptography;
using System.Text;

namespace CRSAPIMAuthenticationDemo.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private static List<TokenRecord> TokenRecords = new List<TokenRecord>();
        private readonly AzureAdOptions _azureOptions;

        public TokenController(IOptions<AzureAdOptions> azureOptions)
        {
            _azureOptions = azureOptions.Value;
        }

        [HttpPost("generate")]
        public IActionResult GenerateEncryptedToken([FromBody] string macAddress)
        {
            if (string.IsNullOrEmpty(macAddress))
                return BadRequest("MAC address is required");

            var salt = "1836-2854-1090";
            var encryptedToken = EncryptToken(macAddress, salt);

            var tokenRecord = new TokenRecord
            {
                Id = TokenRecords.LastOrDefault()?.Id ?? 1,
                MacAddress = macAddress,
                EncryptedToken = encryptedToken,
                CreatedAt = DateTime.UtcNow
            };

            TokenRecords.Add(tokenRecord);

            return Ok(new { EncryptedToken = encryptedToken });
        }

        [HttpGet]
        [Route("validate")]
        public async Task<IActionResult> ValidateToken([FromBody] string encryptedToken)
        {
            var tokenRecord = TokenRecords
                .FirstOrDefault(x => x.EncryptedToken == encryptedToken);

            if (tokenRecord == null)
                return Unauthorized("Token not found");


            var accessToken = await GetAzureOAuthToken();

            return Ok(new { AccessToken = accessToken });
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
            var scopes = new string[] 
            { 
                //"https://graph.microsoft.com/.default" 
            };

            var cca = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
                .Build();

            var result = await cca.AcquireTokenForClient(scopes).ExecuteAsync();
            return result.AccessToken;
        }
    }
}
