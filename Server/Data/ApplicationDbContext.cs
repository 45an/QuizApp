using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QuizApp.Server.Models;

namespace QuizApp.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        public DbSet<Quiz>? Quizzes { get; set; }
        public DbSet<Game>? Games { get; set; }
        public DbSet<Question>? Questions { get; set; }
        public DbSet<Media>? Media { get; set; }
        public DbSet<ApplicationUser> ApplicationUserModel { get; set; }
    }
}