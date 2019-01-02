using System.Collections.Generic;

namespace BeAsBee.Domain.Common {
    public class PageResult<T> where T : class {
        public IEnumerable<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int Count { get; set; }
    }
}