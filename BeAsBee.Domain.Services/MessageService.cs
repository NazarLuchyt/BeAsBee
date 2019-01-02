using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeAsBee.Domain.Common;
using BeAsBee.Domain.Entities;
using BeAsBee.Domain.Interfaces.Services;
using BeAsBee.Infrastructure.UnitOfWork;

namespace BeAsBee.Domain.Services {
    public class MessageService : IMessageService {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService ( IUnitOfWork unitOfWork ) {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> DeleteAsync ( Guid id ) {
            try {
                if ( !await _unitOfWork.MessageRepository.ExistsAsync( i => i.Id == id ) ) {
                    //   throw new ItemNotFoundException( string.Format( Translations.ENTITY_WITH_ID_NOT_FOUND, Translations.Message, id ) );
                }

                await _unitOfWork.MessageRepository.DeleteAsync( id );
                await _unitOfWork.SaveChangesAsync();
                return new OperationResult {IsSuccess = true};
            } catch ( Exception ex ) {
                return new OperationResult( ex );
            }
        }

        public async Task<PageResult<MessageEntity>> GetPagedAsync ( int count = 100, int page = 0 ) {
            var listItems = await _unitOfWork.MessageRepository.GetPagedAsync( count, page );
            var countItems = await _unitOfWork.MessageRepository.CountAsync();
            return new PageResult<MessageEntity> {Items = new List<MessageEntity>( listItems ), Count = countItems, PageNumber = page};
        }

        public async Task<List<MessageEntity>> GetByChatId ( Guid id ) {
            var listItems = await _unitOfWork.MessageRepository.GetByChatId(id);
            //var countItems = await _unitOfWork.MessageRepository.CountAsync();
            return listItems; //new PageResult<MessageEntity> { Items = new List<MessageEntity>(listItems), Count = countItems, PageNumber = page };
        }

        public async Task<MessageEntity> GetByIdAsync ( Guid id ) {
            var result = await _unitOfWork.MessageRepository.GetByIdAsync( id );
            if ( result == null ) {
                //   throw new ItemNotFoundException( string.Format( Translations.ENTITY_WITH_ID_NOT_FOUND, Translations.Message, id ) );
            }

            return result;
        }

        public async Task<OperationResult<MessageEntity>> CreateAsync ( MessageEntity entity ) {
            try {
                var result = await _unitOfWork.MessageRepository.CreateWithSaveAsync( entity );
                return new OperationResult<MessageEntity> {Value = result, IsSuccess = true};
            } catch ( Exception ex ) {
                return new OperationResult<MessageEntity>( ex );
            }
        }

        public async Task<OperationResult> UpdateAsync ( Guid id, MessageEntity entity ) {
            try {
                var entityInDb = await _unitOfWork.MessageRepository.GetByIdAsync( id );
                if ( entityInDb == null ) {
                    //     throw new ItemNotFoundException( string.Format( Translations.ENTITY_WITH_ID_NOT_FOUND, Translations.Message, id ) );
                }

                await _unitOfWork.MessageRepository.UpdateAsync( id, entity );
                await _unitOfWork.SaveChangesAsync();
                return new OperationResult {IsSuccess = true};
            } catch ( Exception ex ) {
                return new OperationResult( ex );
            }
        }
    }
}