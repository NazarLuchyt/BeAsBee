using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BeAsBee.Domain.Common.Exceptions;
using BeAsBee.Domain.Entities;
using BeAsBee.Domain.Resources;
using BeAsBee.Infrastructure.Repositories;
using BeAsBee.Infrastructure.Sql.Common;
using BeAsBee.Infrastructure.Sql.Models.Context;
using BeAsBee.Infrastructure.Sql.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeAsBee.Infrastructure.Sql.Repositories {
    public class UserRepository : GenericRepository<UserEntity, User, Guid>, IUserRepository {
        private readonly ApplicationContext _context;
        private readonly DbSet<User> _dbSet;
        private readonly UserManager<User> _userManager;

        public UserRepository ( ApplicationContext context, UserManager<User> userManager ) : base( context ) {
            _context = context;
            _dbSet = _context.Set<User>();
            _userManager = userManager;
        }

        public async Task<List<UserEntity>> GetPagedAsync ( int count = 10, int page = 0,
                                                            string infoToSearch = null ) {
            var result = await GetAsync( new List<Expression<Func<User, bool>>> {user => (user.FirstName + " " + user.SecondName).Contains( infoToSearch )} )
                .Skip( page * count ).Take( count )
                .ToListAsync();
            return Mapper.Map<List<UserEntity>>( result );
        }

        public async Task<int> CountAsync ( string infoToSearch ) {
            var result = await GetAsync( new List<Expression<Func<User, bool>>> {user => (user.FirstName + user.SecondName).Contains( infoToSearch )} ).CountAsync();
            return result;
        }

        public override async Task<UserEntity> GetByIdAsync ( Guid id ) {
            var result = await GetAsync( new List<Expression<Func<User, bool>>> {i => i.Id == id}, user => user.Include( u => u.UserChats ).ThenInclude( uChat => uChat.Chat ) ).FirstOrDefaultAsync();
            return Mapper.Map<UserEntity>( result );
        }

        #region Identity

        public async Task<UserEntity> FindByNameAsync ( string userName ) {
            var result = await _userManager.FindByNameAsync( userName );
            return Mapper.Map<UserEntity>( result );
        }

        public async Task<IList<string>> GetRolesAsync ( UserEntity userModel ) {
            var result = await _userManager.GetRolesAsync( Mapper.Map<User>( userModel ) );
            return result;
        }

        public async Task<bool> CheckPasswordAsync ( string userName, string password ) {
            var user = await _userManager.FindByNameAsync( userName );
            if ( user == null ) {
                throw new ItemNotFoundException( string.Format( Translations.LOGIN_DOES_NOT_EXIST ) );
            }

            var result = await _userManager.CheckPasswordAsync( user, password );
            return result;
        }

        #endregion
    }
}