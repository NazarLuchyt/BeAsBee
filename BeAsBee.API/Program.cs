using BeAsBee.API.Areas.v1.Data;
using BeAsBee.Infrastructure.Sql.Models.Context;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BeAsBee.API {
    public class Program {
        public static void Main ( string[] args ) {
            var host = BuildWebHost( args );
            using ( var scope = host.Services.CreateScope() ) {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationContext>();
                SeedData.Initialize( context );
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