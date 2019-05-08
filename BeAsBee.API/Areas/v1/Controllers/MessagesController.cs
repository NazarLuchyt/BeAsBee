using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BeAsBee.API.Areas.v1.Common;
using BeAsBee.API.Areas.v1.Models.Common;
using BeAsBee.API.Areas.v1.Models.Message;
using BeAsBee.Domain.Entities;
using BeAsBee.Domain.Interfaces.Services;
using BeAsBee.Domain.Resources;
using BeAsBee.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeAsBee.API.Areas.v1.Controllers {
    [Produces( "application/json" )]
    [Route( "api/v1/[controller]" )]
    public class MessagesController : BaseController {
        private readonly IMessageService _messageService;

        public MessagesController ( IMessageService caseStudyService, IMapper mapper ) : base( mapper ) {
             _messageService = caseStudyService;
        }

        /// <summary>
        ///     Get messages by chat id.
        /// </summary>
        /// <remarks>This route will get entity.</remarks>
        /// <param name="id">Case study`s id.</param>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByChat ( Guid id ) {
            var result = await _messageService.GetByChatId(id);
            var viewModel = Mapper.Map<List<MessageViewModel>>(result);
            return Ok(viewModel);
        }

        ///// <summary>
        /////     Get case study entity by id.
        ///// </summary>
        ///// <remarks>This route will get entity.</remarks>
        ///// <param name="id">Case study`s id.</param>
        //[HttpGet]
        //[Route( "{id:int}" )]
        //public async Task<IActionResult> GetById ( Guid id ) {
        //    var result = await  _messageService.GetByIdAsync( id );
        //    var viewModel = Mapper.Map<MessageViewModel>( result );
        //    return Ok( viewModel );
        //}

        /// <summary>
        ///     Get page with case studies by number of case studies at one page.
        /// </summary>
        /// <remarks>This route will get entities.</remarks>
        /// <param name="count">Number of items per page.</param>
        /// <param name="page">Number of page.</param>
        [HttpGet]
        public async Task<IActionResult> GetPage ( int count, int page ) {
            if ( count == 0 ) {
                throw new ArgumentException( Translations.COUNT_CANNOT_BE_NULL );
            }

            var result = await  _messageService.GetPagedAsync( count, page );
            var viewModels = Mapper.Map<PageResultViewModel<MessageViewModel>>( result );
            return Ok( viewModels );
        }

        /// <summary>
        ///     Create new case study and add in DB.
        /// </summary>
        /// <remarks>This route will create entity.</remarks>
        /// <param name="model">Message model to create.</param>
        [HttpPost]
        public async Task<IActionResult> Create ( [FromBody] CreateMessageBindingModel model ) {
            if ( !ModelState.IsValid ) {
                return BadRequest(ModelState);
            }

            var modelEntity = Mapper.Map<MessageEntity>(model);
            var result = await _messageService.CreateAsync(modelEntity);
            if ( !result.IsSuccess ) {
                throw result.Exception;
            }

            var viewModel = Mapper.Map<MessageViewModel>(result.Value);
            return Ok(viewModel);
        }

        ///// <summary>
        /////     Update case study entity.
        ///// </summary>
        ///// <remarks>This route will update entity.</remarks>
        ///// <param name="id">Case study`s id.</param>
        ///// <param name="model">Case study model to update.</param>
        //[HttpPut]
        //public async Task<IActionResult> Update ( int id, [FromBody] UpdateMessageBindingModel model ) {
        //    if ( !ModelState.IsValid ) {
        //        return BadRequest(ModelState);
        //    }

        //    var modelEntity = Mapper.Map<MessageEntity>(model);
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
            var result = await  _messageService.DeleteAsync( id );
            if ( !result.IsSuccess ) {
                throw result.Exception;
            }

            return Ok();
        }
    }
}