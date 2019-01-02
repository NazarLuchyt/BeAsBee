using System.Threading.Tasks;
using AutoMapper;
using BeAsBee.API.Areas.v1.Common;
using BeAsBee.API.Areas.v1.Models.User;
using BeAsBee.Domain.Common.Exceptions;
using BeAsBee.Domain.Entities;
using BeAsBee.Domain.Interfaces.Services;
using BeAsBee.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeAsBee.API.Areas.v1.Controllers {
    [Produces( "application/json" )]
    [Route( "api/v1/[controller]" )]
    public class AccountController : BaseController {
        private readonly IUserService _userService;

        public AccountController ( IUserService userService, IMapper mapper ) : base( mapper ) {
            _userService = userService;
        }

        /// <summary>
        ///     Post case study entity by id.
        /// </summary>
        /// <remarks>This route will get entity.</remarks>
        /// <param name="model">Login model.</param>
        [HttpPost]
        [Route( "login" )]
        public async Task<IActionResult> Login ( [FromBody] LoginBindingModel model ) {
            var result = await _userService.GetByEmail( model.Email, model.Password );
            if ( result == null ) {
                throw new ItemNotFoundException( "Email or password is incorrect!" );
            }

            var viewModel = _mapper.Map<UserViewModel>( result );
            return Ok( viewModel );
        }

        /// <summary>
        ///     Create new user.
        /// </summary>
        /// <remarks>This route will create entity.</remarks>
        /// <param name="model">Registration model.</param>
        [HttpPost]
        [HttpPost]
        [Route( "registration" )]
        public async Task<IActionResult> Registration ( [FromBody] CreateUserBindingModel model ) {
            if ( !ModelState.IsValid ) {
                return BadRequest( ModelState );
            }

            var modelEntity = _mapper.Map<UserEntity>( model );
            var result = await _userService.CreateAsync( modelEntity );
            if ( !result.IsSuccess ) {
                throw result.Exception;
            }

            var viewModel = _mapper.Map<UserViewModel>( result.Value );
            return Ok( viewModel );
        }
    }
}