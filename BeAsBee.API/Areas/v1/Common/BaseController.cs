using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeAsBee.API.Areas.v1.Common {
    [Authorize( AuthenticationSchemes = "Bearer" )]
    //  [AuthorizeRoles(RoleType.User, RoleType.Admin)]
    public class BaseController : Controller {
        protected readonly IMapper Mapper;

        public BaseController ( IMapper mapper ) {
            Mapper = mapper;
        }
    }
}