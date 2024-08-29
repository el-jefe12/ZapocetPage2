using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TexasGuyContractIdentity.Models;
using TexasGuyContractIdentity.Models.API;
using TexasGuyContractIdentity.Pages;

namespace TexasGuyContractIdentity.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<History> HistoryEntries { get; set; }
        public DbSet<Stations> StationsEntries { get; set; }
        public DbSet<Users> UsersEntries { get; set; }
        public DbSet<ConfigModel.SmtpModel> SmtpModels { get; set; }
        public DbSet<ApiToken> ApiTokens { get; set; }

    }
}
