using System;
using System.Threading.Tasks;
using BeAsBee.Domain.Common;
using BeAsBee.Domain.Entities;

namespace BeAsBee.Domain.Interfaces.Services {
    public interface IUserService {
        Task<PageResult<UserEntity>> GetPagedAsync ( int count, int page, string infoToSearch );
        Task<UserEntity> GetByIdAsync ( Guid id );
        Task<UserEntity> GetByEmail ( string email, string password );
        Task<OperationResult<UserEntity>> CreateAsync ( UserEntity entity );
        Task<OperationResult> UpdateAsync ( Guid id, UserEntity entity );
        Task<OperationResult> DeleteAsync ( Guid id );
    }
}