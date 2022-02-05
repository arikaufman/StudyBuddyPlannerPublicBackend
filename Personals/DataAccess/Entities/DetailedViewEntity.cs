using System;
using System.Collections.Generic;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Personals.Domain.DomainObjects;

namespace plannerBackEnd.Personals.DataAccess.Entities
{
    public class DetailedViewEntity : IMaps<DetailedView>
    {
        public string TaskType { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Subject { get; set; } = "";
        public string SubjectColor { get; set; } =  "";
        public string SubjectDescription { get; set; } = "";
        public int TotalMinutes { get; set; } = 0;
        /*TEMPORARY*/ public double SessionMinutes { get; set; } = 0;
        /*TEMPORARY*/ public DateTime DateCompleted { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now;
        public List<DetailedViewItemEntity> SessionItems { get; set; } = null;
        /*TEMPORARY*/ public int DateDifference { get; set; } = 0;
        /*TEMPORARY*/ public double MinutePercentage { get; set; } = 0;

    }
    public class DetailedViewItemEntity : IMaps<DetailedViewItem>
    {
        public double SessionMinutes { get; set; } = 0;
        public DateTime DateCompleted { get; set; } = DateTime.Now;
        public int DateDifference { get; set; } = 0;
        public double MinutePercentage { get; set; } = 0;

    }
}

