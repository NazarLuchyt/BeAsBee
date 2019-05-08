using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeAsBee.API.Areas.v1.Common;
using BeAsBee.API.Areas.v1.Models.Common;
using BeAsBee.API.Areas.v1.Models.Enums;
using BeAsBee.API.Areas.v1.Models.User;
using BeAsBee.Domain.Interfaces.Services;
using BeAsBee.Domain.Resources;
using BeAsBee.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeAsBee.API.Areas.v1.Controllers {
    [Produces( "application/json" )]
    [Route( "api/v1/[controller]" )]
    public class UsersController : BaseController {
        private readonly IUserService _userService;
        private readonly IChatService _chatService;

        public UsersController ( IChatService chatService, IUserService userService, IMapper mapper ) : base( mapper ) {
            _userService = userService;
            _chatService = chatService;
        }

        /// <summary>
        ///     Get user entity by id.
        /// </summary>
        /// <remarks>This route will get entity.</remarks>
        /// <param name="id">User`s id.</param>
        /// <param name="userViewType">Type information about user for view.</param>
        [HttpGet]
        [Route( "{id},{userViewType}" )]
        public async Task<IActionResult> GetById ( Guid id, UserViewTypeEnum userViewType ) {
            var result = await _userService.GetByIdAsync( id );

            if ( userViewType == UserViewTypeEnum.Home ) {
                return Ok( Mapper.Map<UserPageBindingModel>( result ) );
            }

            // var result = await _userService.GetByIdAsync( id );
            // viewModel = Mapper.Map<UserViewModel>( result );
            return Ok();
        }

        /// <summary>
        ///     Get page with case studies by number of case studies at one page.
        /// </summary>
        /// <remarks>This route will get entities.</remarks>
        /// <param name="count">Number of items per page.</param>
        /// <param name="page">Number of page.</param>
        /// <param name="infoToSearch">Key words to find user.</param>
        [HttpGet]
        public async Task<IActionResult> GetPage ( int count, int page, string infoToSearch ) {
            if ( count == 0 ) {
                throw new ArgumentException( Translations.COUNT_CANNOT_BE_NULL );
            }

            var result = await _userService.GetPagedAsync( count, page, infoToSearch );
            var viewModels = Mapper.Map<PageResultViewModel<UserPageBindingModel>>( result );
            return Ok( viewModels );
        }

        ///// <summary>
        /////     Create new user and add in DB.
        ///// </summary>
        ///// <remarks>This route will create entity.</remarks>
        ///// <param name="model">Case Study model to create.</param>
        //[HttpPost]
        //public async Task<IActionResult> Create ( [FromBody] CreateUserBindingModel model ) {
        //    if ( !ModelState.IsValid ) {
        //        return BadRequest( ModelState );
        //    }

        //    var modelEntity = Mapper.Map<UserEntity>( model );
        //    var result = await _userService.CreateAsync( modelEntity );
        //    if ( !result.IsSuccess ) {
        //        throw result.Exception;
        //    }

        //    var viewModel = Mapper.Map<UserViewModel>( result.Value );
        //    return Ok( viewModel );
        //}

        ///// <summary>
        /////     Update user entity.
        ///// </summary>
        ///// <remarks>This route will update entity.</remarks>
        ///// <param name="id">User`s id.</param>
        ///// <param name="model">Case study model to update.</param>
        //[HttpPut]
        //public async Task<IActionResult> Update ( Guid id, [FromBody] UpdateUserBindingModel model ) {
        //    if ( !ModelState.IsValid ) {
        //        return BadRequest( ModelState );
        //    }

        //    var modelEntity = Mapper.Map<UserEntity>( model );
        //    var result = await _userService.UpdateAsync( id, modelEntity );
        //    if ( !result.IsSuccess ) {
        //        throw result.Exception;
        //    }

        //    return Ok();
        //}

        /// <summary>
        ///     Delete user entity by Id.
        /// </summary>
        /// <remarks>This route will delete entity.</remarks>
        /// <param name="id">Case Study`s id.</param>
        [HttpDelete]
        public async Task<IActionResult> Delete ( Guid id ) {
            var result = await _userService.DeleteAsync( id );
            if ( !result.IsSuccess ) {
                throw result.Exception;
            }

            return Ok();
        }
    }
}