using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BeAsBee.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace BeAsBee.Infrastructure.Common {
    public interface IGenericRepository<TEntity, in TKey>
        where TEntity : IEntity<TKey> {
        Task<List<TEntity>> GetPagedAsync ( int count = 10, int page = 0,
                                            Expression<Func<TEntity, bool>> filter = null,
                                            params Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>[] includes );

        Task<TEntity> GetByIdAsync ( List<Expression<Func<TEntity, bool>>> filters, params Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>[] includes );
        Task<TEntity> GetByIdAsync ( TKey id, params Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>[] includes );
        Task<TEntity> GetByIdAsync ( TKey id );
        Task<bool> ExistsAsync ( Expression<Func<TEntity, bool>> filter );
        Task<int> CountAsync ( params Expression<Func<TEntity, bool>>[] filters );
        Task CreateAsync ( TEntity entity );
        Task<TEntity> CreateWithSaveAsync ( TEntity entity );
        Task DeleteAsync ( TKey id );
        Task UpdateAsync ( TKey id, TEntity entity );
    }
}