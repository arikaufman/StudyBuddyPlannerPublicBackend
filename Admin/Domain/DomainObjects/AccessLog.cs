using System;

namespace plannerBackEnd.Admin.Domain.DomainObjects
{
    public class AccessLog
    {
        public int Id { get; set; } = 0;
        public string HttpMethod { get; set; } = "";
        public string HttpRequest { get; set; } = "";
        public string Url { get; set; } = "";
        public int User { get; set; } = 0;
        public int Duration { get; set; } = 0;

    }
}