using CRSAPIMAuthenticationDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace CRSAPIMAuthenticationDemo.Data
{
    public class TokenDbContext : DbContext
    {
        public TokenDbContext(DbContextOptions<TokenDbContext> options) : base(options)
        {

        }

        public DbSet<TokenRecord> TokenRecords { get; set; }
    }
}
