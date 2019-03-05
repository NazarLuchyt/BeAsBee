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
                Role = role,
                UserFirstName = user.FirstName,
                UserSecondName = user.SecondName
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

        /// <summary>
        ///     Create new user.
        /// </summary>
        /// <remarks>This route will create entity.</remarks>
        /// <param name="model">Registration model.</param>
        [HttpPost]
        [AllowAnonymous]
        [Route( "registration" )]
        public async Task<IActionResult> Registration ( [FromBody] CreateUserBindingModel model ) {
            if ( await _userService.FindByNameAsync( model.UserName ) != null ) {
                ModelState.AddModelError( "UserName", Translations.EMAIL_ALREADY_EXIST );
            }

            if ( !ModelState.IsValid ) {
                return BadRequest( ModelState );
            }

            var modelEntity = _mapper.Map<UserEntity>( model );
            var result = await _userService.CreateAsync( modelEntity, model.Password );
            if ( !result.IsSuccess ) {
                var errList = "";
                var error = result.Exception.Message.Split( "|" ).ToList();

                foreach ( var err in error ) {
                    if ( !string.IsNullOrEmpty( err ) ) {
                        ModelState.AddModelError( "Password", err );
                    }
                }
                return BadRequest( ModelState );
            }

            return Ok();
        }

        //// PUT api/values/5
        //[HttpPost]
        //[Route( "register" )]
        //public async Task<IActionResult> Register ( [FromBody] RegisterBindingModel model ) {
        //    if ( !ModelState.IsValid ) { return BadRequest( "Could not create token" ); }

        //    if ( await _unitOfWork.UserManager.FindByNameAsync( model.Email ) != null ) {
        //        ModelState.AddModelError( "Email", "ERR_USER_ALREADY_EXISTS" );
        //    } else {
        //        var user = new User {UserName = model.Email, Email = model.Email, LastName = model.LastName, Name = model.Name, GenderType = model.Gender};
        //        var result = await _unitOfWork.UserManager.CreateAsync( user, model.Password );
        //        if ( !result.Succeeded ) { return BadRequest( "Could not create token" ); }

        //        var claims = new[] {
        //            new Claim( ClaimsIdentity.DefaultNameClaimType, user.UserName ),
        //            new Claim( JwtRegisteredClaimNames.Sub, user.Email ),
        //            new Claim( JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() ),
        //            new Claim( JwtRegisteredClaimNames.Sid, user.Id ) // Set userid to token Sid claim
        //        };

        //        var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( _options.Value.Key ) );
        //        var creds = new SigningCredentials( key, SecurityAlgorithms.HmacSha256 );

        //        var token = new JwtSecurityToken( _options.Value.Issuer,
        //            _options.Value.Issuer,
        //            claims,
        //            expires: DateTime.Now.AddMinutes( 30 ),
        //            signingCredentials: creds );

        //        return Ok( new {token = new JwtSecurityTokenHandler().WriteToken( token ), id = user.Id} );
        //    }

        //    return BadRequest( "Could not create token" );
        //}

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