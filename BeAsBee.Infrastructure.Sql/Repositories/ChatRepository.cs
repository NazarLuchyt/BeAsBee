using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BeAsBee.Domain.Entities;
using BeAsBee.Infrastructure.Repositories;
using BeAsBee.Infrastructure.Sql.Common;
using BeAsBee.Infrastructure.Sql.Helpers;
using BeAsBee.Infrastructure.Sql.Models;
using BeAsBee.Infrastructure.Sql.Models.Context;
using BeAsBee.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BeAsBee.Infrastructure.Sql.Repositories {
    public class ChatRepository : GenericRepository<ChatEntity, Chat, Guid>, IChatRepository {
        private readonly ApplicationContext _context;
        private readonly DbSet<Chat> _dbSet;

        public ChatRepository ( ApplicationContext context ) : base( context ) {
            _context = context;
            _dbSet = _context.Set<Chat>();
        }

        public override async Task<ChatEntity> GetByIdAsync ( Guid id ) {
            var result = await GetAsync( new List<Expression<Func<Chat, bool>>> {i => i.Id == id},
                    c => c.Include( chat => chat.UserChats ).ThenInclude( uChat => uChat.User ),
                    c => c.Include( chat => chat.Messages ) )
                .FirstOrDefaultAsync();
            return Mapper.Map<ChatEntity>( result );
        }

        public async Task<List<ChatEntity>> GetPagedAsync ( Guid userId, int count = 10, int page = 0 ) {
            var filters = new List<Expression<Func<Chat, bool>>> {chat => chat.UserChats.Any( user => user.UserId == userId )};
            var result = await GetAsync( filters, chat => chat.Include( c => c.UserChats ).ThenInclude( u => u.User ) )
                .Skip( page * count )
                .Take( count )
                .ToListAsync();

            return Mapper.Map<List<ChatEntity>>( result );
        }

        public async Task<int> CountAsync ( Guid userId ) {
            var filters = new List<Expression<Func<Chat, bool>>> {chat => chat.UserChats.Any( user => user.UserId == userId )};
            var result = await GetAsync( filters ).CountAsync();
            return result;
        }

        public async Task AddUsersAsync ( Guid chatId, List<Guid> newUserGuids ) {
            var currentChat = await GetAsync( new List<Expression<Func<Chat, bool>>> {i => i.Id == chatId},
                    c => c.Include( chat => chat.UserChats ).ThenInclude( uChat => uChat.User ) )
                .FirstOrDefaultAsync(); // TODO check is variable null

            var newChatUsers = newUserGuids.Select( userGuid => new UserChat {ChatId = currentChat.Id, UserId = userGuid} );

            var toAddUsers = newChatUsers.Except( currentChat.UserChats, key => key.UserId ).ToList();

            if ( !toAddUsers.Any() ) {
                throw new ArgumentException( "Unique users are not found!" );
            }

            currentChat.UserChats.AddRange( toAddUsers );
        }

        public async Task<List<UserEntity>> RemoveUsersAsync ( Guid chatId, List<Guid> removeUserGuids ) {
            var currentChat = await GetAsync( new List<Expression<Func<Chat, bool>>> {i => i.Id == chatId},
                    c => c.Include( chat => chat.UserChats ).ThenInclude( uChat => uChat.User ) )
                .FirstOrDefaultAsync(); // TODO check is variable null
            var removedUsers = new List<UserEntity>();

            foreach ( var removeUser in removeUserGuids ) {
                var userToRemove = currentChat.UserChats.Find( uChat => uChat.UserId == removeUser );
                if ( userToRemove == null ) {
                    continue;
                }

                currentChat.UserChats.Remove( userToRemove );
                removedUsers.Add( new UserEntity {
                    FirstName = userToRemove.User.FirstName,
                    SecondName = userToRemove.User.SecondName
                } );
            }

            return removedUsers;
        }

        // TODO future feature
        public async Task AddUsersAsync1 ( Guid chatId, List<Guid> newUserGuids ) {
            var currentChat = await GetAsync( new List<Expression<Func<Chat, bool>>> {i => i.Id == chatId},
                    c => c.Include( chat => chat.UserChats ).ThenInclude( uChat => uChat.User ) )
                .FirstOrDefaultAsync(); // TODO check is variable null

            // -------------------------- TODO delete
            var newChatUsers = newUserGuids.Select( userGuid => new UserChat {ChatId = currentChat.Id, UserId = userGuid} );

            // -------------------------- TODO delete
            var temp = newChatUsers.ToList();
            temp.AddRange( currentChat.UserChats );
            var k = temp;
            // -----------------
            _context.UpdateManyToMany( currentChat.UserChats, temp, userChat => userChat.UserId );

            //  await _context.SaveChangesAsync();

            //var updatedModel = Mapper.Map<Vacancy>(updatedEntity);

            //_context.Entry(currentEntity).CurrentValues.SetValues(updatedModel);
        }
    }
}