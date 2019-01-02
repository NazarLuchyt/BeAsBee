using Autofac;
using BeAsBee.Domain.Interfaces.Services;
using BeAsBee.Domain.Services;

namespace BeAsBee.IoC {
    internal class DomainModule : Module {
        protected override void Load ( ContainerBuilder builder ) {
            // Register domain services.
            builder.RegisterType<MessageService>().As<IMessageService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<ChatService>().As<IChatService>();
        }
    }
}