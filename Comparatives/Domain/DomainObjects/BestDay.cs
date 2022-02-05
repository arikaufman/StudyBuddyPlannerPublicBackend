using System;

namespace plannerBackEnd.Comparatives.Domain.DomainObjects
{
    public class BestDay
    {
        public int Id { get; set; } = 0;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public double Minutes { get; set; } = 0;
        public DateTime BestDayDate { get; set; } = DateTime.Now;

    }
}