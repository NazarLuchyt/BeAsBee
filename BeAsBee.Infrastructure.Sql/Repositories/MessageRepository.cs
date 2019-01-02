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
using Microsoft.EntityFrameworkCore.Query;

namespace BeAsBee.Infrastructure.Sql.Repositories {
    public class MessageRepository : GenericRepository<MessageEntity, Message, Guid>, IMessageRepository {
        private readonly ApplicationContext _context;
        private readonly DbSet<Message> _dbSet;

        public MessageRepository ( ApplicationContext context ) : base( context ) {
            _context = context;
            _dbSet = _context.Set<Message>();
        }

        //public override async Task<List<MessageEntity>> GetPagedAsync ( int count = 10, int page = 0,
        //                                                                Expression<Func<TEntity, bool>> filter = null,
        //                                                                params Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>[] includes ) {
        //    var result = await GetAsync(new List<Expression<Func<Chat, bool>>> { i => i.Id == id },
        //            c => c.Include(chat => chat.UserChats).ThenInclude(uChat => uChat.User),
        //            c => c.Include(chat => chat.Messages))
        //        .FirstOrDefaultAsync();
        //    return Mapper.Map<ChatEntity>(result);
        //}

        public override async Task<List<MessageEntity>> GetPagedAsync ( int count = 10, int page = 0,
                                                                        Expression<Func<MessageEntity, bool>> filter = null,
                                                                        params Expression<Func<IQueryable<MessageEntity>, IIncludableQueryable<MessageEntity, object>>>[] includes ) {
            var result = await GetAsync(
                    filter != null ? new List<Expression<Func<Message, bool>>> {Mapper.Map<Expression<Func<Message, bool>>>( filter )} : null )
                .OrderByDescending( message => message.ReceivedTime )
                .Skip( page * count )
                .Take( count )
                .ToListAsync();

            return Mapper.Map<List<MessageEntity>>( result );
        }

        //public virtual async Task<List<TEntity>> GetPagedAsync ( int count = 10, int page = 0,
        //                                                         Expression<Func<TEntity, bool>> filter = null,
        //                                                         params Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>[] includes ) {
        //    var filtersSql = Mapper.Map<Expression<Func<T, bool>>>(filter);
        //    var includesSql = Mapper.Map<List<Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>>>>(includes).ToArray();

        //    return await GetAllPagedAsync(filter != null ? new List<Expression<Func<T, bool>>> { filtersSql } : null,
        //        count, page, includesSql);
        //}

        //public async Task<int> CountAsync ( Guid userId ) {
        //    var filters = new List<Expression<Func<Chat, bool>>> { chat => chat.UserChats.Any(user => user.UserId == userId) };
        //    var result = await GetAsync(filters).CountAsync();
        //    return result;
        //}

        public async Task<List<MessageEntity>> GetByChatId ( Guid id ) {
            var page = 0;
            var count = 100;
            var filters = new List<Expression<Func<Message, bool>>> {message => message.ChatId == id};
            var result = await GetAsync( filters ).OrderBy( message => message.ReceivedTime )
                .Skip( page * count )
                .Take( count )
                .ToListAsync();
            return Mapper.Map<List<MessageEntity>>( result );
        }

        //public async Task<int> CountAsync ( List<string> filterExpertises = null,
        //                                    List<string> filterServices = null,
        //                                    List<string> filterTechnologies = null,
        //                                    List<string> filterIndustries = null ) {
        //    var result = await GetAsync( GetFilters( filterExpertises, filterServices, filterTechnologies, filterIndustries ) ).CountAsync();
        //    return result;
        //}

        //public override async Task<List<MessageEntity>> GetPagedAsync ( int count = 10, int page = 0,
        //                                                         Expression<Func<MessageEntity, bool>> filter = null,
        //                                                         params Expression<Func<IQueryable<MessageEntity>, IIncludableQueryable<MessageEntity, object>>>[] includes ) {
        //    var filterSql = Mapper.Map<Expression<Func<Message, bool>>>(filter);
        //    var filters = new List<Expression<Func<MessageEntity, bool>>>() {
        //        message => message.ChatId == id
        //        //new Expression<Func<MessageEntity, bool>>()
        //    };
        //    var includesSql = Mapper.Map<List<Expression<Func<IQueryable<Message>, IIncludableQueryable<Message, object>>>>>(includes).ToArray();

        //   var temp = await GetAsync( filtersSql, includesSql );
        //        count, page, includesSql);
        //        .Skip(page * count).Take(count).ToListAsync();
        //}
    }
}