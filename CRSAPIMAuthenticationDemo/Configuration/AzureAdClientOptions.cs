namespace CRSAPIMAuthenticationDemo.Configuration
{
    public class AzureAdClientOptions
    {
        public string ClientId { get; set; } = String.Empty;
        public string ClientSecrets { get; set; } = String.Empty;
        public string Scope { get; set; } = String.Empty;
    }
}
