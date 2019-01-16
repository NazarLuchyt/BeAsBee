using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BeAsBee.API.Areas.v1.Common;
using BeAsBee.API.Areas.v1.Models.Enums;
using BeAsBee.API.Areas.v1.Models.User;
using BeAsBee.Domain.Common;
using BeAsBee.Domain.Entities;
using BeAsBee.Domain.Interfaces.Services;
using BeAsBee.Domain.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BeAsBee.API.Areas.v1.Controllers {
    //[Produces( "application/json" )]
    //[Route( "api/v1/[controller]" )]
    //public class AccountController : BaseController {
    //    private readonly IUserService _userService;

    //    public AccountController ( IUserService userService, IMapper mapper ) : base( mapper ) {
    //        _userService = userService;
    //    }

    //    /// <summary>
    //    ///     Post case study entity by id.
    //    /// </summary>
    //    /// <remarks>This route will get entity.</remarks>
    //    /// <param name="model">Login model.</param>
    //    [HttpPost]
    //    [Route( "login" )]
    //    public async Task<IActionResult> Login ( [FromBody] LoginBindingModel model ) {
    //        var result = await _userService.GetByEmail( model.Email, model.Password );
    //        if ( result == null ) {
    //            throw new ItemNotFoundException( "Email or password is incorrect!" );
    //        }

    //        var viewModel = _mapper.Map<UserViewModel>( result );
    //        return Ok( viewModel );
    //    }

    //    /// <summary>
    //    ///     Create new user.
    //    /// </summary>
    //    /// <remarks>This route will create entity.</remarks>
    //    /// <param name="model">Registration model.</param>
    //    [HttpPost]
    //    [HttpPost]
    //    [Route( "registration" )]
    //    public async Task<IActionResult> Registration ( [FromBody] CreateUserBindingModel model ) {
    //        if ( !ModelState.IsValid ) {
    //            return BadRequest( ModelState );
    //        }

    //        var modelEntity = _mapper.Map<UserEntity>( model );
    //        var result = await _userService.CreateAsync( modelEntity );
    //        if ( !result.IsSuccess ) {
    //            throw result.Exception;
    //        }

    //        var viewModel = _mapper.Map<UserViewModel>( result.Value );
    //        return Ok( viewModel );
    //    }
    //}
    [Produces( "application/json" )]
    [Route( "api/v1/accounts" )]
    public class AccountsController : BaseController {
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IUserService _userService;

        public AccountsController ( IJwtService jwtFactory, IUserService userService, IOptions<JwtIssuerOptions> jwtOptions, IConfiguration configuration, IMapper mapper ) : base( mapper ) {
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _configuration = configuration;
            _userService = userService;
        }

        /// <summary>
        ///     Login
        /// </summary>
        /// <remarks>This route will get entity.</remarks>
        /// <param name="credentials">Credentials to login.</param>
        [AllowAnonymous]
        [HttpPost( "login" )]
        public async Task<IActionResult> Login ( [FromBody] LoginBindingModel credentials ) {
            var identity = await GetClaimsIdentity( credentials.UserName, credentials.Password );
            if ( identity == null ) { return BadRequest( new {message = Translations.LOGIN_DOES_NOT_EXIST} ); }

            var user = await _userService.FindByNameAsync( credentials.UserName );
            var role = await GetUserRole( user );
            var response = new {
                Token = await _jwtFactory.GenerateEncodedToken( user.Id.ToString(), identity, role.ToString() ),
                Role = role
            };
            return Ok( response );
        }

        private async Task<RoleType> GetUserRole ( string userName ) {
            var user = await _userService.FindByNameAsync( userName );
            var role = await _userService.GetRolesAsync( user );
            return Enum.Parse<RoleType>( role.FirstOrDefault() );
        }

        private async Task<RoleType> GetUserRole ( UserEntity user ) {
            var role = await _userService.GetRolesAsync( user );
            return Enum.Parse<RoleType>( role.FirstOrDefault() );
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity ( string userName, string password ) {
            if ( string.IsNullOrEmpty( userName ) || string.IsNullOrEmpty( password ) ) {
                return await Task.FromResult<ClaimsIdentity>( null );
            }

            if ( await _userService.CheckPasswordAsync( userName, password ) ) {
                var role = await GetUserRole( userName );
                return await Task.FromResult( _jwtFactory.GenerateClaimsIdentity( userName, role.ToString() ) );
            }

            return await Task.FromResult<ClaimsIdentity>( null );
        }

        //[AllowAnonymous]
        //[HttpPut("change-password")]
        //public async Task<IActionResult> ChangePassword ( [FromBody]UserChangePasswordModel model ) {
        //    if ( model.NewPassword == model.ConfirmNewPassword ) {
        //        var identityUser = await _userManager.FindByNameAsync(this.User.Identity.Name);
        //        if ( await _userManager.CheckPasswordAsync(identityUser, model.OldPassword) ) {
        //            var result = await _userManager.ChangePasswordAsync(identityUser, model.OldPassword, model.NewPassword);
        //            if ( result.Succeeded ) {
        //                return Ok();
        //            } else { return BadRequest(result.Errors); }
        //        }
        //    }
        //    return BadRequest();
        //}

        //[AllowAnonymous]
        //[HttpPost("generatetoken")]
        //public async Task<IActionResult> ForgotPassword ( string email ) {
        //    if ( email == null ) { return BadRequest(); }
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if ( user == null ) {
        //        var errorMessage = Translations.EMAIL_DOES_NOT_EXIST;
        //        return StatusCode((int) HttpStatusCode.Conflict, new { errorMessage });
        //    }
        //    var token = _userManager.GeneratePasswordResetTokenAsync(user);
        //    var encodeToken = HttpUtility.UrlEncode(token.Result);
        //    var frontAddress = _configuration.GetSection("FileSetting").GetSection("FRONT_ADDRESS").Value;
        //    var message = string.Format(MailConstants.UserNotificationAfterForgotPassword, frontAddress, encodeToken, user.UserName);
        //    MailingRepository.ForgotPasswordNotification(_dependencyResolver, message, email);
        //    return Ok();
        //}

        //[AllowAnonymous]
        //[HttpPost("reset-password")]
        //public async Task<IActionResult> SetNewPassword ( [FromBody]NewPasswordModel newPassword ) {
        //    if ( newPassword.NewPassword == newPassword.ConfirmPassword ) {
        //        var decodeToken = newPassword.Token;
        //        var user = await _userManager.FindByNameAsync(newPassword.UserName);
        //        if ( user == null ) { return BadRequest(); }
        //        var result = await _userManager.ResetPasswordAsync(user, decodeToken, newPassword.NewPassword);
        //        if ( result.Succeeded ) { return Ok(); } else { return BadRequest(result.Errors); }
        //    }
        //    return BadRequest();
        //}
    }
}