using System;
using System.Threading.Tasks;
using BeAsBee.Infrastructure.Repositories;

namespace BeAsBee.Infrastructure.UnitOfWork {
    public interface IUnitOfWork : IDisposable {
        IMessageRepository MessageRepository { get; }
        IUserRepository UserRepository { get; }
        IChatRepository ChatRepository { get; }
        Task SaveChangesAsync ();
    }
}