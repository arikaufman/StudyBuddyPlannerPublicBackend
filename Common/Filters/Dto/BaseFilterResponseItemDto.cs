using System;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Common.Filters.DomainObjects;

namespace plannerBackEnd.Common.Filters.Dto
{
    public class BaseFilterResponseItemDto : IMaps<BaseFilterResponseItem>
    {
        public string Name1 { get; set; } = "";
        public string Name2 { get; set; } = "";
        public DateTime Date1 { get; set; } = DateTime.Now;
        public double Value1 { get; set; } = 0;
        public double Value2 { get; set; } = 0;

    }
}