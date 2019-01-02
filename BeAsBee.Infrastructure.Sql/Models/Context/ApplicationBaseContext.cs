using Microsoft.EntityFrameworkCore;

namespace BeAsBee.Infrastructure.Sql.Models.Context {
    public abstract class ApplicationBaseContext : DbContext /*IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, UserRoleClaim, UserToken>*/ {
        public ApplicationBaseContext ( DbContextOptions<ApplicationContext> options )
            : base( options ) {
        }
    }
}