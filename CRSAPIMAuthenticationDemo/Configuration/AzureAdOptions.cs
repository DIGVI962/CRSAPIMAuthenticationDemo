namespace CRSAPIMAuthenticationDemo.Configuration
{
    public class AzureAdOptions
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecrets { get; set; }
        public string Issuer {get; set; }
        public string Audience { get; set; }
        public List<string> scopes { get; set; }
    }
}
