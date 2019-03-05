using System;
using System.Linq;
using BeAsBee.API.Areas.v1.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace BeAsBee.API.Filters.AuthorizeRolesAttribute {
    public class AuthorizeRolesAttribute : AuthorizeAttribute {
        public AuthorizeRolesAttribute ( params RoleType[] allowedRoles ) {
            var allowedRolesAsStrings = allowedRoles.Select( x => Enum.GetName( typeof( RoleType ), x ) );
            Roles = string.Join( ",", allowedRolesAsStrings );
        }
    }
}