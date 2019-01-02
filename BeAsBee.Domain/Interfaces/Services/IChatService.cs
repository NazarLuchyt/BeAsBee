﻿using System;
using System.Threading.Tasks;
using BeAsBee.Domain.Common;
using BeAsBee.Domain.Entities;

namespace BeAsBee.Domain.Interfaces.Services {
    public interface IChatService {
        Task<PageResult<ChatEntity>> GetPagedAsync ( Guid userId, int countMessage, int page, int count );
        Task<ChatEntity> GetByIdAsync ( Guid id );
        Task<OperationResult<ChatEntity>> CreateAsync ( ChatEntity entity );
        Task<OperationResult> UpdateAsync ( Guid id, ChatEntity entity );
        Task<OperationResult> DeleteAsync ( Guid id );
    }
}