namespace CRSAPIMAuthenticationDemo.Models
{
    public class TokenRecord
    {
        public int Id { get; set; }
        public string MacAddress { get; set; } = String.Empty;
        public string EncryptedToken { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
    }
}
