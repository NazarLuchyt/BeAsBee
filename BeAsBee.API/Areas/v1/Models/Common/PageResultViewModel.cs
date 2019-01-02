using System.Collections.Generic;

namespace BeAsBee.API.Areas.v1.Models.Common {
    public class PageResultViewModel<T> where T : class {
        public IEnumerable<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int Count { get; set; }
    }
}