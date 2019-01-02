﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeAsBee.Domain.Common;
using BeAsBee.Domain.Entities;
using BeAsBee.Domain.Interfaces.Services;
using BeAsBee.Infrastructure.UnitOfWork;

namespace BeAsBee.Domain.Services {
    public class ChatService : IChatService {
        private readonly IUnitOfWork _unitOfWork;

        public ChatService ( IUnitOfWork unitOfWork ) {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> DeleteAsync ( Guid id ) {
            try {
                if ( !await _unitOfWork.ChatRepository.ExistsAsync( i => i.Id == id ) ) {
                    //   throw new ItemNotFoundException( string.Format( Translations.ENTITY_WITH_ID_NOT_FOUND, Translations.Chat, id ) );
                }

                await _unitOfWork.ChatRepository.DeleteAsync( id );
                await _unitOfWork.SaveChangesAsync();
                return new OperationResult {IsSuccess = true};
            } catch ( Exception ex ) {
                return new OperationResult( ex );
            }
        }

        public async Task<PageResult<ChatEntity>> GetPagedAsync ( Guid userId,int countMessage, int count = 100, int page = 0 ) {
            var listItems = await _unitOfWork.ChatRepository.GetPagedAsync( userId, count, page );
            foreach ( var chat in listItems ) {
                chat.Messages = (await _unitOfWork.MessageRepository.GetPagedAsync(countMessage, 0, m => m.ChatId == chat.Id));
                //string[] temp = new string[100];
                //int i = 0;
                //foreach ( var message in chat.Messages ) {
                //    temp[i] = message.ReceivedTime.ToString();
                //    i++;
                //}

                //var tem1p = temp;
                chat.Messages.Reverse();
            }
            var countItems = await _unitOfWork.ChatRepository.CountAsync( userId );

            return new PageResult<ChatEntity> {Items = new List<ChatEntity>( listItems ), Count = countItems, PageNumber = page};
        }

        public async Task<ChatEntity> GetByIdAsync ( Guid id ) {
            var result = await _unitOfWork.ChatRepository.GetByIdAsync( id );
            if ( result == null ) {
                //   throw new ItemNotFoundException( string.Format( Translations.ENTITY_WITH_ID_NOT_FOUND, Translations.Chat, id ) );
            }

            return result;
        }

        public async Task<OperationResult<ChatEntity>> CreateAsync ( ChatEntity entity ) {
            try {
                var result = await _unitOfWork.ChatRepository.CreateWithSaveAsync( entity );
                return new OperationResult<ChatEntity> {Value = result, IsSuccess = true};
            } catch ( Exception ex ) {
                return new OperationResult<ChatEntity>( ex );
            }
        }

        public async Task<OperationResult> UpdateAsync ( Guid id, ChatEntity entity ) {
            try {
                var entityInDb = await _unitOfWork.ChatRepository.GetByIdAsync( id );
                if ( entityInDb == null ) {
                    //     throw new ItemNotFoundException( string.Format( Translations.ENTITY_WITH_ID_NOT_FOUND, Translations.Chat, id ) );
                }

                await _unitOfWork.ChatRepository.UpdateAsync( id, entity );
                await _unitOfWork.SaveChangesAsync();
                return new OperationResult {IsSuccess = true};
            } catch ( Exception ex ) {
                return new OperationResult( ex );
            }
        }
    }
}