using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeAsBee.Domain.Entities;
using BeAsBee.Infrastructure.Common;

namespace BeAsBee.Infrastructure.Repositories {
    public interface IMessageRepository : IGenericRepository<MessageEntity, Guid> {
        Task<List<MessageEntity>> GetByChatId ( Guid id );
    }
}