using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeAsBee.API.Areas.v1.Common {
    [Authorize( AuthenticationSchemes = "Bearer" )]
    public class BaseController : Controller {
        protected IMapper _mapper;

        public BaseController ( IMapper mapper ) {
            _mapper = mapper;
        }
    }
}