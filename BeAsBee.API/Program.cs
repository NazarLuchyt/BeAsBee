using BeAsBee.API.Areas.v1.Data;
using BeAsBee.Infrastructure.Sql.Models.Context;
using BeAsBee.Infrastructure.Sql.Models.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BeAsBee.API {
    public class Program {
        public static void Main ( string[] args ) {
            var host = BuildWebHost( args );
            using ( var scope = host.Services.CreateScope() ) {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationContext>();
                var roleManager = services.GetRequiredService<RoleManager<Role>>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                SeedData.Initialize( context, roleManager, userManager ).Wait();
            }

            host.Run();
        }

        public static IWebHost BuildWebHost ( string[] args ) {
            return WebHost.CreateDefaultBuilder( args )
                .UseStartup<Startup>()
                .Build();
        }
    }
}

//The INSERT statement conflicted with the FOREIGN KEY constraint "FK_Chats_AspNetUsers_UserId". The conflict occurred in database "BeAsBee", table "dbo.AspNetUsers", column 'Id'.
//The statement has been terminated.
//The statement has been terminated.