namespace CRSAPIMAuthenticationDemo.Configuration
{
    public class AzureAdClientOptions
    {
        public string Instance { get; set; } = String.Empty;
        public string TenantId { get; set; } = String.Empty;
        public string ClientId { get; set; } = String.Empty;
        public string ClientSecrets { get; set; } = String.Empty;
        public string Scope { get; set; } = String.Empty;
    }
}
