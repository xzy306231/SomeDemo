using LearnApp.Shared.Enum;
using System;
using System.Collections.Generic;

namespace LearnApp.Shared.BaseBusinessDto
{
    public class FdSearchDto
    {
        public long Id { get; protected set; }

        public DateTime? Begin { get; set; }

        public DateTime? End { get; set; }

        public IList<string> PrimaryKeys { get; set; }

        public virtual Pagination Pagination { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
    public class Pagination : SortConfifure
    {
        public int PageIndex { get; set; } = 1;


        public int PageSize { get; set; } = 25;

    }
    public class SortConfifure
    {
        public string Field { get; set; }

        public SortBy sortBy { get; set; }
    }
}
