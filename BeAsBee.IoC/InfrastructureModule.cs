using Autofac;
using BeAsBee.Infrastructure.Repositories;
using BeAsBee.Infrastructure.Sql.Models.Context;
using BeAsBee.Infrastructure.Sql.Repositories;
using BeAsBee.Infrastructure.Sql.UnitOfWork;
using BeAsBee.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BeAsBee.IoC {
    internal class InfrastructureModule : Module {
        protected override void Load ( ContainerBuilder builder ) {
            // Register infrastructure dependencies.
            builder.RegisterType<ApplicationContext>().As<DbContext>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            builder.RegisterType<MessageRepository>().As<IMessageRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<ChatRepository>().As<IChatRepository>();
        }
    }
}