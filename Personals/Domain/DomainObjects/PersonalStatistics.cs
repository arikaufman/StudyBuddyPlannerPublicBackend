namespace plannerBackEnd.Personals.Domain.DomainObjects
{
    public class PersonalStatistics
    {
        public double CurrentDay { get; set; } = 0;
        public double CurrentWeek { get; set; } = 0;
        public double CurrentMonth { get; set; } = 0;
        public double AverageDay { get; set; } = 0;
        public double AverageWeek { get; set; } = 0;
        public double AverageMonth { get; set; } = 0;
        public double BestDay { get; set; } = 0;
        public double BestWeek { get; set; } = 0;
        public double BestMonth { get; set; } = 0;

    }
}