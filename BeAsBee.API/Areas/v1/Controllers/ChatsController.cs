using System;
using System.Threading.Tasks;
using AutoMapper;
using BeAsBee.API.Areas.v1.Common;
using BeAsBee.API.Areas.v1.Models.Chat;
using BeAsBee.API.Areas.v1.Models.Common;
using BeAsBee.API.Areas.v1.Models.Enums;
using BeAsBee.API.Filters.AuthorizeRolesAttribute;
using BeAsBee.Domain.Entities;
using BeAsBee.Domain.Interfaces.Services;
using BeAsBee.Domain.Resources;
using Microsoft.AspNetCore.Mvc;

namespace BeAsBee.API.Areas.v1.Controllers {
    [Produces( "application/json" )]
    [Route( "api/v1/[controller]" )]
    public class ChatsController : BaseController {
        private readonly IChatService _chatService;

        public ChatsController ( IChatService chatService, IMapper mapper ) : base( mapper ) {
            _chatService = chatService;
        }

        /// <summary>
        ///     Get chat entity by id.
        /// </summary>
        /// <remarks>This route will get entity.</remarks>
        /// <param name="id">Chat`s id.</param>
        [HttpGet]
        [Route( "{id}" )]
        public async Task<IActionResult> GetById ( Guid id ) {
            var result = await _chatService.GetByIdAsync( id );
            var viewModel = _mapper.Map<ChatViewModel>( result );
            return Ok( viewModel );
        }

        /// <summary>
        ///     Get page with chats by number of chats at one page.
        /// </summary>
        /// <remarks>This route will get entities.</remarks>
        /// <param name="count">Number of items per page.</param>
        /// <param name="page">Number of page.</param>
        /// <param name="userId">User id to filtered chats.</param>
        /// <param name="countMessage">Number of message for the first load.</param>
        [HttpGet]
        public async Task<IActionResult> GetPage ( [FromQuery] Guid userId, [FromQuery] int countMessage, [FromQuery] int count, [FromQuery] int page ) {
            if ( count == 0 ) {
                throw new ArgumentException( Translations.COUNT_CANNOT_BE_NULL );
            }

            var result = await _chatService.GetPagedAsync( userId, countMessage, count, page );
            var viewModels = _mapper.Map<PageResultViewModel<ChatViewModel>>( result );
            return Ok( viewModels );
        }

        /// <summary>
        ///     Create new chat and add in DB.
        /// </summary>
        /// <remarks>This route will create entity.</remarks>
        /// <param name="model">Chat model to create.</param>
        [HttpPost]
        public async Task<IActionResult> Create ( [FromBody] CreateChatBindingModel model ) {
            if ( !ModelState.IsValid ) {
                return BadRequest( ModelState );
            }

            var modelEntity = _mapper.Map<ChatEntity>( model );
            var result = await _chatService.CreateAsync( modelEntity );
            if ( !result.IsSuccess ) {
                throw result.Exception;
            }

            var viewModel = _mapper.Map<ChatViewModel>( result.Value );
            return Ok( viewModel );
        }

        ///// <summary>
        /////     Update case study entity.
        ///// </summary>
        ///// <remarks>This route will update entity.</remarks>
        ///// <param name="id">Case study`s id.</param>
        ///// <param name="model">Case study model to update.</param>
        //[HttpPut]
        //public async Task<IActionResult> Update ( int id, [FromBody] UpdateChatBindingModel model ) {
        //    if ( !ModelState.IsValid ) {
        //        return BadRequest(ModelState);
        //    }

        //    var modelEntity = _mapper.Map<ChatEntity>(model);
        //    var result = await  _messageService.UpdateAsync(id, modelEntity);
        //    if ( !result.IsSuccess ) {
        //        throw result.Exception;
        //    }

        //    return Ok();
        //}

        /// <summary>
        ///     Delete case study entity by Id.
        /// </summary>
        /// <remarks>This route will delete entity.</remarks>
        /// <param name="id">Case Study`s id.</param>
        [HttpDelete]
        public async Task<IActionResult> Delete ( Guid id ) {
            var result = await _chatService.DeleteAsync( id );
            if ( !result.IsSuccess ) {
                throw result.Exception;
            }

            return Ok();
        }
    }
}