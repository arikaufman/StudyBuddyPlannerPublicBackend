using System;
using Common.Enums;

namespace plannerBackEnd.Common.DomainObjects
{
    public class RequestContext
    {
        public int UserId { get; set; } = 0;
        public DateTime RequestStartTime { get; set; } = DateTime.Now;
        public string ClientIp { get; set; } = "";
    }
}