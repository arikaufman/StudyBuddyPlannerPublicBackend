using plannerBackEnd.Common.automapper;
using plannerBackEnd.Common.Filters.DomainObjects;
using System.Collections.Generic;

namespace plannerBackEnd.Common.Filters.Dto
{
    public class BaseFilterResponseDto : IMaps<BaseFilterResponse>
    {
        public string Title { get; set; } = "";
        public string Color { get; set; } = "";
        public List<BaseFilterResponseItemDto> ResponseItems { get; set; } = new List<BaseFilterResponseItemDto>();
    }
}
