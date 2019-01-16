using System;
using System.Threading.Tasks;
using BeAsBee.Infrastructure.Repositories;
using BeAsBee.Infrastructure.Sql.Models.Context;
using BeAsBee.Infrastructure.Sql.Models.Identity;
using BeAsBee.Infrastructure.Sql.Repositories;
using BeAsBee.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace BeAsBee.Infrastructure.Sql.UnitOfWork {
    public class UnitOfWork : IUnitOfWork {
        private readonly ApplicationContext _context;

        private readonly bool _disposed = false;
        private readonly UserManager<User> _userManager;

        public UnitOfWork ( ApplicationContext context, UserManager<User> userManager ) {
            _context = context;
            _userManager = userManager;
        }

        public async Task SaveChangesAsync () {
            await _context.SaveChangesAsync();
        }

        public void Dispose () {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        protected virtual void Dispose ( bool disposing ) {
            if ( !_disposed ) {
                if ( disposing ) {
                    _context.Dispose();
                }
            }
        }

        #region Repositories

        private IMessageRepository _messageRepository;

        public IMessageRepository MessageRepository {
            get { return _messageRepository = _messageRepository ?? new MessageRepository( _context ); }
        }

        private IUserRepository _userRepository;

        public IUserRepository UserRepository {
            get { return _userRepository = _userRepository ?? new UserRepository( _context, _userManager ); }
        }

        private IChatRepository _chatRepository;

        public IChatRepository ChatRepository {
            get { return _chatRepository = _chatRepository ?? new ChatRepository( _context ); }
        }

        #endregion
    }
}