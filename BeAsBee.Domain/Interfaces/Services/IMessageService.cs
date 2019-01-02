using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeAsBee.Domain.Common;
using BeAsBee.Domain.Entities;

namespace BeAsBee.Domain.Interfaces.Services {
    public interface IMessageService {
        Task<PageResult<MessageEntity>> GetPagedAsync ( int page, int count );
        Task<MessageEntity> GetByIdAsync ( Guid id );
        Task<List<MessageEntity>> GetByChatId ( Guid id );
        Task<OperationResult<MessageEntity>> CreateAsync ( MessageEntity entity );
        Task<OperationResult> UpdateAsync ( Guid id, MessageEntity entity );
        Task<OperationResult> DeleteAsync ( Guid id );
    }
}