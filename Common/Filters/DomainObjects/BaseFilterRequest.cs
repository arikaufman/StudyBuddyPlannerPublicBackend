using System;
using Common.Enums;

namespace plannerBackEnd.Common.Filters.DomainObjects
{
    public class BaseFilterRequest
    { 
        public int UserId { get; set; } = 0;
        public int SchoolId { get; set; } = 0;
        public int SubjectId { get; set; } = 0;
        public int SemesterId { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Now;
        public BreakdownType BreakdownType { get; set; } = BreakdownType.Faculty;
        public string Tasktype { get; set; } = "";
        public bool Personal { get; set; } = false;
    }
}
