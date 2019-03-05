using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeAsBee.Domain.Entities;
using BeAsBee.Infrastructure.Common;

namespace BeAsBee.Infrastructure.Repositories {
    public interface IUserRepository : IGenericRepository<UserEntity, Guid> {
        Task<List<UserEntity>> GetPagedAsync ( int count = 10, int page = 0,
                                               string infoToSearch = null );

        Task<int> CountAsync ( string infoToSearch = null );

        #region Identity

        Task<UserEntity> FindByNameAsync ( string userName );
        Task<IList<string>> GetRolesAsync ( UserEntity userModel );
        Task<bool> CheckPasswordAsync ( string userName, string password );
        Task<bool> CreateAsync ( UserEntity userModel, string password );

        #endregion
    }
}