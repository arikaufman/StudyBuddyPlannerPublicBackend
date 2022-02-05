using System;
using System.Collections.Generic;

namespace plannerBackEnd.Common.Filters.DomainObjects
{
    public class BaseFilterResponse
    {
        public string Title { get; set; } = "";
        public string Color { get; set; } = "";
        public List<BaseFilterResponseItem> ResponseItems { get; set; } = new List<BaseFilterResponseItem>();
    }
}