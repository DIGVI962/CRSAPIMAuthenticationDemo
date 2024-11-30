namespace CRSAPIMAuthenticationDemo.Configuration
{
    public class AzureAdOptions
    {
        public string TenantId { get; set; } = String.Empty;
        public string ClientId { get; set; } = String.Empty;
        public string ClientSecrets { get; set; } = String.Empty;
        public string Issuer {get; set; } = String.Empty;
        public string Audience { get; set; } = String.Empty;
        public List<string> scopes { get; set; } = new List<string>();
    }
}
