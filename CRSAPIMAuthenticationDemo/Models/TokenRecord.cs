namespace CRSAPIMAuthenticationDemo.Models
{
    public class TokenRecord
    {
        public int Id { get; set; }
        public string MacAddress { get; set; }
        public string EncryptedToken { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
