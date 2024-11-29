namespace CRSAPIMAuthenticationDemo.Configuration
{
    public class AzureAdOptions
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecrets { get; set; }
        public string Issuer {get; set;}
    }
}
