using AutoMapper;
using BeAsBee.API.Areas.v1.Models.Enums;
using BeAsBee.API.Filters.AuthorizeRolesAttribute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeAsBee.API.Areas.v1.Common {
    [Authorize( AuthenticationSchemes = "Bearer" )]
  //  [AuthorizeRoles(RoleType.User, RoleType.Admin)]
    public class BaseController : Controller {
        protected IMapper _mapper;

        public BaseController ( IMapper mapper ) {
            _mapper = mapper;
        }
    }
}