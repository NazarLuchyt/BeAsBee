using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeAsBee.Domain.Common;
using BeAsBee.Domain.Common.Exceptions;
using BeAsBee.Domain.Entities;
using BeAsBee.Domain.Interfaces.Services;
using BeAsBee.Infrastructure.UnitOfWork;

namespace BeAsBee.Domain.Services {
    public class UserService : IUserService {
        private readonly IUnitOfWork _unitOfWork;

        public UserService ( IUnitOfWork unitOfWork ) {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> DeleteAsync ( Guid id ) {
            try {
                if ( !await _unitOfWork.UserRepository.ExistsAsync( i => i.Id == id ) ) {
                    //   throw new ItemNotFoundException( string.Format( Translations.ENTITY_WITH_ID_NOT_FOUND, Translations.User, id ) );
                }

                await _unitOfWork.UserRepository.DeleteAsync( id );
                await _unitOfWork.SaveChangesAsync();
                return new OperationResult {IsSuccess = true};
            } catch ( Exception ex ) {
                return new OperationResult( ex );
            }
        }

        public async Task<UserEntity> GetByIdAsync ( Guid id ) {
            var result = await _unitOfWork.UserRepository.GetByIdAsync( id );
            if ( result == null ) {
                //   throw new ItemNotFoundException( string.Format( Translations.ENTITY_WITH_ID_NOT_FOUND, Translations.User, id ) );
            }

            return result;
        }

        public async Task<OperationResult<UserEntity>> CreateAsync ( UserEntity entity ) {
            try {
                var result = await _unitOfWork.UserRepository.CreateWithSaveAsync( entity );
                return new OperationResult<UserEntity> {Value = result, IsSuccess = true};
            } catch ( Exception ex ) {
                return new OperationResult<UserEntity>( ex );
            }
        }

        public async Task<OperationResult> UpdateAsync ( Guid id, UserEntity entity ) {
            try {
                var entityInDb = await _unitOfWork.UserRepository.GetByIdAsync( id );
                if ( entityInDb == null ) {
                    //     throw new ItemNotFoundException( string.Format( Translations.ENTITY_WITH_ID_NOT_FOUND, Translations.User, id ) );
                }

                await _unitOfWork.UserRepository.UpdateAsync( id, entity );
                await _unitOfWork.SaveChangesAsync();
                return new OperationResult {IsSuccess = true};
            } catch ( Exception ex ) {
                return new OperationResult( ex );
            }
        }

        public async Task<UserEntity> GetByEmail ( string email, string password ) {
            var result = await _unitOfWork.UserRepository.GetByEmail( email );
            if ( result != null && string.Equals( result.Password, password ) ) {
                return result;
            }

            throw new ItemNotFoundException( "Email or password is incorrect!" );
        }

        public async Task<PageResult<UserEntity>> GetPagedAsync ( int count = 100, int page = 0, string infoToSearch = null ) {
            var listItems = await _unitOfWork.UserRepository.GetPagedAsync( count, page, infoToSearch);
            var countItems = await _unitOfWork.UserRepository.CountAsync(infoToSearch);
            return new PageResult<UserEntity> {Items = new List<UserEntity>( listItems ), Count = countItems, PageNumber = page};
        }
    }
}