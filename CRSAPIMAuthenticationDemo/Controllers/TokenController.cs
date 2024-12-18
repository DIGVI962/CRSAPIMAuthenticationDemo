﻿using CRSAPIMAuthenticationDemo.Configuration;
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
        private static List<TokenRecord> TokenRecords = new List<TokenRecord>();
        private readonly AzureAdClientOptions _azureClientOptions;

        public TokenController(IOptions<AzureAdClientOptions> azureClientOptions)
        {
            _azureClientOptions = azureClientOptions.Value;
        }

        [HttpPost("generate")]
        public IActionResult GenerateEncryptedToken([FromBody] GenerateTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.MacAddress))
                return BadRequest("MAC address is required");

            var salt = "1836-2854-1090";
            var encryptedToken = EncryptToken(request.MacAddress, salt);

            var storedValue = TokenRecords.FirstOrDefault(x => x.EncryptedToken == encryptedToken);
            if (storedValue != null)
            {
                return Ok(new { EncryptedToken = storedValue.EncryptedToken });
            }

            var tokenRecord = new TokenRecord
            {
                EncryptedToken = encryptedToken,
                CreatedAt = DateTime.UtcNow
            };

            TokenRecords.Add(tokenRecord);

            return Ok(new { EncryptedToken = encryptedToken });
        }

        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenRequest request)
        {
            var tokenRecord = TokenRecords
                .FirstOrDefault(x => x.EncryptedToken == request.EncryptedToken);

            if (tokenRecord == null)
                return Unauthorized("Please Generate an Identification Token first");

            try
            {
                var accessToken = await GetAzureOAuthToken();
                return Ok(new { AccessToken = accessToken });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: (int)HttpStatusCode.InternalServerError, detail: ex.Message);
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
            var clientId = _azureClientOptions.ClientId;
            var clientSecret = _azureClientOptions.ClientSecrets;
            var scope = _azureClientOptions.Scope;
            var instance = _azureClientOptions.Instance;
            var tenantId = _azureClientOptions.TenantId;

            var cca = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority($"{instance}{tenantId}/")
                .Build();

            var result = await cca.AcquireTokenForClient(new List<string> { scope }).ExecuteAsync();
            return result.AccessToken;
        }
    }
}
