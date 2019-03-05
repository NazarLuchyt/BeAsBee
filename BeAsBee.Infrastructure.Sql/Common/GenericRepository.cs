using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BeAsBee.Domain.Common.Exceptions;
using BeAsBee.Domain.Entities;
using BeAsBee.Domain.Resources;
using BeAsBee.Infrastructure.Common;
using BeAsBee.Infrastructure.Sql.Models.Context;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BeAsBee.Infrastructure.Sql.Common {
    public abstract class GenericRepository<TEntity, T, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
        where T : class {
        private readonly ApplicationContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository ( ApplicationContext context ) {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<TEntity> GetByIdAsync ( TKey id ) {
            var entitySql = await _dbSet.FindAsync( id );

            return Mapper.Map<TEntity>( entitySql );
        }

        public virtual async Task<TEntity> CreateWithSaveAsync ( TEntity entity ) {
            var entitySql = Mapper.Map<T>( entity );
            await _dbSet.AddAsync( entitySql );
            await _context.SaveChangesAsync();
            return Mapper.Map<TEntity>( entitySql );
        }

        public virtual async Task CreateAsync ( TEntity entity ) {
            await _dbSet.AddAsync( Mapper.Map<T>( entity ) );
        }

        public virtual async Task DeleteAsync ( TKey id ) {
            var result = await _dbSet.FindAsync( id );
            if ( result == null ) {
                throw new ItemNotFoundException( string.Format( Translations.ENTITY_WITH_ID_NOT_FOUND, Translations.ResourceManager.GetString( typeof( T ).Name ) ?? typeof( T ).Name, id ) );
            }

            _dbSet.Remove( result );
        }

        public async Task<bool> ExistsAsync ( Expression<Func<TEntity, bool>> filter ) {
            return await _dbSet.AnyAsync( Mapper.Map<Expression<Func<T, bool>>>( filter ) );
        }

        public virtual async Task UpdateAsync ( TKey id, TEntity entity ) {
            var result = await _dbSet.FindAsync( id );
            if ( result == null ) {
                //   throw new ItemNotFoundException( string.Format( Translations.ENTITY_WITH_ID_NOT_FOUND, Translations.ResourceManager.GetString( typeof( T ).Name ) ?? typeof( T ).Name, id ) );
            }

            _context.Entry( result ).CurrentValues.SetValues( Mapper.Map<T>( entity ) );
        }

        public async Task<TEntity> GetByIdAsync ( List<Expression<Func<TEntity, bool>>> filters,
                                                  params Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>[] includes ) {
            var filtersSql = Mapper.Map<List<Expression<Func<T, bool>>>>( filters );
            var includesSql = Mapper.Map<List<Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>>>>( includes );
            return Mapper.Map<TEntity>( await GetAsync( filtersSql, includesSql.ToArray() ).FirstOrDefaultAsync() );
        }

        public async Task<TEntity> GetByIdAsync ( TKey id, params Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>[] includes ) {
            return await GetByIdAsync( i => i.Id.ToString() == id.ToString(), includes );
        }

        protected IQueryable<T> GetAsync ( List<Expression<Func<T, bool>>> filters = null,
                                           params Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>>[] includes ) {
            IQueryable<T> query = _dbSet;
            if ( !includes.IsNullOrEmpty() ) {
                query = includes.Select( i => i.Compile() ).Aggregate( query, ( list, next ) => query = next( query ) );
            }

            if ( !filters.IsNullOrEmpty() ) {
                foreach ( var filter in filters ) {
                    query = query.Where( filter );
                }
            }

            return query;
        }

        private async Task<TEntity> GetByIdAsync ( Expression<Func<TEntity, bool>> filter,
                                                   params Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>[] includes ) {
            return await GetByIdAsync( filter != null ? new List<Expression<Func<TEntity, bool>>> {filter} : null, includes );
        }

        #region Pagination

        private async Task<List<TEntity>> GetAllPagedAsync ( List<Expression<Func<T, bool>>> filters = null,
                                                             int count = 10, int page = 0,
                                                             params Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>>[] includes ) {
            var result = await GetAsync( filters, includes ).Skip( page * count ).Take( count ).ToListAsync();

            return Mapper.Map<List<TEntity>>( result );
        }

        public virtual async Task<List<TEntity>> GetPagedAsync ( int count = 10, int page = 0,
                                                                 Expression<Func<TEntity, bool>> filter = null,
                                                                 params Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>[] includes ) {
            var filtersSql = Mapper.Map<Expression<Func<T, bool>>>( filter );
            var includesSql = Mapper.Map<List<Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>>>>( includes ).ToArray();

            return await GetAllPagedAsync( filter != null ? new List<Expression<Func<T, bool>>> {filtersSql} : null,
                count, page, includesSql );
        }

        public async Task<int> CountAsync ( params Expression<Func<TEntity, bool>>[] filters ) {
            var filterSql = Mapper.Map<List<Expression<Func<T, bool>>>>( filters );
            return await GetAsync( filters != null ? new List<Expression<Func<T, bool>>>( filterSql ) : null )
                .CountAsync();
        }

        #endregion
    }
}