using plannerBackEnd.Common.automapper;

namespace plannerBackEnd.Schools.Controllers.Dto
{
    public class SchoolDto : IMaps<School>
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Region { get; set; } = "";

        //Generated Fields
        public double NumberOfStudents { get; set; } = 0;
    }
}