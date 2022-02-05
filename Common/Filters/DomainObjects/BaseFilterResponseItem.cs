using System;

namespace plannerBackEnd.Common.Filters.DomainObjects
{
    public class BaseFilterResponseItem
    {
        public string Name1 { get; set; } = "";
        public string Name2 { get; set; } = "";
        public DateTime Date1 { get; set; } = DateTime.Now;
        public double Value1 { get; set; } = 0;
        public double Value2 { get; set; } = 0;

    }
}
