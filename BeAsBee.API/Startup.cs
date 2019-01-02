using System;
using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using BeAsBee.API.Hubs;
using BeAsBee.Domain.Resources;
using BeAsBee.Infrastructure.Sql.Models.Context;
using BeAsBee.Infrastructure.Sql.UnitOfWork;
using BeAsBee.Infrastructure.UnitOfWork;
using BeAsBee.IoC;
using JSPWebsite.WebAPI.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace BeAsBee.API {
    public class Startup {
        public Startup ( IConfiguration configuration ) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices ( IServiceCollection services ) {
            services.AddMvc();
            services.AddCors( options => {
                options.AddPolicy( "CorsPolicy",
                    builder => builder
                        .WithOrigins( "http://localhost:6321" )
                        .WithOrigins( "http://localhost:4200" )
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials() );
            } );

            #region Translation and GEF

            services.AddLocalization( options => options.ResourcesPath = "BeAsBee.Domain/Resources" );
            services.AddMvc( cfg => {
                    cfg.Filters.Add( typeof( GlobalExceptionFilter ) );
                } )
                .AddDataAnnotationsLocalization( o => {
                    o.DataAnnotationLocalizerProvider = ( type, factory ) =>
                        factory.Create( typeof( Translations ) );
                } );
            services.AddScoped<GlobalExceptionFilter>();

            #endregion

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen( cfg => {
                cfg.SwaggerDoc( "v1", new Info {
                    Version = "v1",
                    Title = "Be As Bee Chat API",
                    Description = "Be As Bee Chat API",
                    TermsOfService = "None"
                } );
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine( AppContext.BaseDirectory, xmlFile );
                cfg.IncludeXmlComments( xmlPath );
            } );
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddAutofac();
            services.AddAutoMapper( cfg => cfg.AddExpressionMapping() );
            services.AddDbContext<ApplicationContext>( options => options.UseSqlServer( Configuration.GetConnectionString( "DefaultConnection" ) ) );
            services.AddSignalR();
            return new AutofacServiceProvider( ContainerManager.BuildContainer( services ) );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure ( IApplicationBuilder app, IHostingEnvironment env ) {
            if ( env.IsDevelopment() ) {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors( "CorsPolicy" );
            app.UseStaticFiles();

            app.UseSignalR( routes => {
                routes.MapHub<ChatHub>( "/chat" );
            } );

            app.UseSwagger();
            app.UseSwaggerUI( c => {
                c.SwaggerEndpoint( "/swagger/v1/swagger.json", "Be As Bee API V1" );
            } );
            app.UseMvc( routes => {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}" );
            } );
        }
    }
}