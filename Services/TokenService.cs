using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TexasGuyContractIdentity.Data;
using TexasGuyContractIdentity.Models.API;

namespace TexasGuyContractIdentity.Services
{
    public class TokenService
    {
        private readonly ApplicationDbContext _context;

        public TokenService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GenerateToken()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<ApiToken> CreateTokenAsync()
        {
            var token = new ApiToken
            {
                Token = GenerateToken(),
                CreatedAt = DateTime.UtcNow
            };

            _context.ApiTokens.Add(token);
            await _context.SaveChangesAsync();

            return token;
        }

        public async Task<ApiToken> GetTokenAsync(string tokenValue)
        {
            return await _context.ApiTokens.FirstOrDefaultAsync(t => t.Token == tokenValue);
        }
    }
}
