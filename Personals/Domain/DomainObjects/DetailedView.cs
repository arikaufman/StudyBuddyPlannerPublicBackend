using System;
using System.Collections.Generic;

namespace plannerBackEnd.Personals.Domain.DomainObjects
{
    public class DetailedView
    {
        public string TaskType { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Subject { get; set; } = "";
        public string SubjectColor { get; set; } = "";
        public string SubjectDescription { get; set; } = "";
        public int TotalMinutes { get; set; } = 0;
        public DateTime DueDate { get; set; } = DateTime.Now;
        public List<DetailedViewItem> SessionItems { get; set; } = null;

    }
    public class DetailedViewItem
    {
        public double SessionMinutes { get; set; } = 0;
        public DateTime DateCompleted { get; set; } = DateTime.Now;
        public int DateDifference { get; set; } = 0;
        public double MinutePercentage { get; set; } = 0;

    }
}