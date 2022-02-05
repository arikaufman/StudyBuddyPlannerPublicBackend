using System;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Semesters.Domain.DomainObjects;

namespace plannerBackEnd.Semesters.Controllers.Dto
{
    public class SemesterDto : IMaps<Semester>
    {
        public int Id { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public double Startgpa { get; set; } = 0;
        public double Startpercentage { get; set; } = 0;
        public int Active { get; set; } = 0;
        public string Title { get; set; } = "";
    }
}