using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeAsBee.Domain.Common;
using BeAsBee.Domain.Entities;

namespace BeAsBee.Domain.Interfaces.Services {
    public interface IUserService {
        Task<PageResult<UserEntity>> GetPagedAsync ( int count, int page, string infoToSearch );
        Task<UserEntity> GetByIdAsync ( Guid id );
        Task<OperationResult<UserEntity>> CreateAsync ( UserEntity entity );
        Task<OperationResult> UpdateAsync ( Guid id, UserEntity entity );
        Task<OperationResult> DeleteAsync ( Guid id );

        #region Identity

        Task<UserEntity> FindByNameAsync ( string userName );
        Task<IList<string>> GetRolesAsync ( UserEntity userModel );
        Task<bool> CheckPasswordAsync ( string userName, string password );

        #endregion
    }
}