using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BeAsBee.Domain.Entities;
using BeAsBee.Infrastructure.Repositories;
using BeAsBee.Infrastructure.Sql.Common;
using BeAsBee.Infrastructure.Sql.Models;
using BeAsBee.Infrastructure.Sql.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace BeAsBee.Infrastructure.Sql.Repositories {
    public class ChatRepository : GenericRepository<ChatEntity, Chat, Guid>, IChatRepository {
        public ChatRepository ( ApplicationContext context ) : base( context ) {
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
            var result = await GetAsync( filters , chat => chat.Include( c => c.UserChats ).ThenInclude( u => u.User ) )
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
    }
}