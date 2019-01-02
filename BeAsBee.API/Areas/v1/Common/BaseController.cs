using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace BeAsBee.API.Areas.v1.Common {
    public class BaseController : Controller {
        protected IMapper _mapper;

        public BaseController ( IMapper mapper ) {
            _mapper = mapper;
        }
    }
}