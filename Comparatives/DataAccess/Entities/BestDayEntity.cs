using System;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Comparatives.Domain.DomainObjects;

namespace plannerBackEnd.Comparatives.DataAccess.Entities
{
    public class BestDayEntity : IMaps<BestDay>
    {
        public int Id { get; set; } = 0;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public double Minutes { get; set; } = 0;
        public DateTime BestDayDate { get; set; } = DateTime.Now;
    }
}