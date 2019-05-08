using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeAsBee.Domain.Entities;
using BeAsBee.Infrastructure.Common;

namespace BeAsBee.Infrastructure.Repositories {
    public interface IChatRepository : IGenericRepository<ChatEntity, Guid> {
        Task<List<ChatEntity>> GetPagedAsync ( Guid userId, int count = 10, int page = 0 );
        Task<int> CountAsync ( Guid userId );
        Task AddUsersAsync ( Guid chatId, List<Guid> newUserGuids );
        Task<List<UserEntity>> RemoveUsersAsync ( Guid chatId, List<Guid> removeUserGuids );
    }
}