using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using BeAsBee.Domain.Common;
using BeAsBee.Domain.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace BeAsBee.Domain.Services {
    public class JwtService : IJwtService {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtService ( IOptions<JwtIssuerOptions> jwtOptions ) {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions( _jwtOptions );
        }

        public async Task<string> GenerateEncodedToken ( string userName, ClaimsIdentity identity, string role ) {
            var claims = new[] {
                new Claim( JwtRegisteredClaimNames.Sub, userName ),
                new Claim( JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator() ),
                new Claim( JwtRegisteredClaimNames.Iat, ToUnixEpochDate( _jwtOptions.IssuedAt ).ToString(), ClaimValueTypes.Integer64 ),
                new Claim( ClaimsIdentity.DefaultNameClaimType, userName ),
                new Claim( "role", role ),
                new Claim( ClaimsIdentity.DefaultRoleClaimType, role )
            };

            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                _jwtOptions.NotBefore,
                _jwtOptions.Expiration,
                _jwtOptions.SigningCredentials );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken( jwt );
            return encodedJwt;
        }

        public ClaimsIdentity GenerateClaimsIdentity ( string userName, string role ) {
            return new ClaimsIdentity( new GenericIdentity( userName, "Token" ), new[] {
                new Claim( ClaimsIdentity.DefaultNameClaimType, userName ),
                new Claim( ClaimsIdentity.DefaultRoleClaimType, role )
            } );
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate ( DateTime date ) {
            return ( long ) Math.Round( (date.ToUniversalTime() -
                                         new DateTimeOffset( 1970, 1, 1, 0, 0, 0, TimeSpan.Zero ))
                .TotalSeconds );
        }

        private static void ThrowIfInvalidOptions ( JwtIssuerOptions options ) {
            if ( options == null ) {
                throw new ArgumentNullException( nameof(options) );
            }

            if ( options.ValidFor <= TimeSpan.Zero ) {
                throw new ArgumentException( "Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor) );
            }

            if ( options.SigningCredentials == null ) {
                throw new ArgumentNullException( nameof(JwtIssuerOptions.SigningCredentials) );
            }

            if ( options.JtiGenerator == null ) {
                throw new ArgumentNullException( nameof(JwtIssuerOptions.JtiGenerator) );
            }
        }
    }
}