namespace CRSAPIMAuthenticationDemo.Models
{
    public class TokenRecord
    {
        public int Id { get; set; }
        public string EncryptedToken { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
    }
}
