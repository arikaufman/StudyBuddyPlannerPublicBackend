using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Personals.Domain.DomainObjects;

namespace plannerBackEnd.Personals.Controllers.Dto
{
    public class DetailedViewDto : IMaps<DetailedView>
    {
        public string TaskType { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Subject { get; set; } = "";
        public string SubjectColor { get; set; } = "";
        public string SubjectDescription { get; set; } = "";
        public int TotalMinutes { get; set; } = 0;
        public DateTime DueDate { get; set; } = DateTime.Now;
        public List<DetailedViewItemDto> SessionItems { get; set; } = null;

    }
    public class DetailedViewItemDto : IMaps<DetailedViewItem>
    {
        public double SessionMinutes { get; set; } = 0;
        public DateTime DateCompleted { get; set; } = DateTime.Now;
        public int DateDifference { get; set; } = 0;
        public double MinutePercentage { get; set; } = 0;

    }
}
