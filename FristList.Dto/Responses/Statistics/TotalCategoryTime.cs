using System;
using System.Collections.Generic;

namespace FristList.Dto.Responses.Statistics
{
    public class TotalCategoryTime : DtoObjectBase
    {
        public DateTime From { get; init; }
        public DateTime To { get; init; }
        
        public IReadOnlyList<KeyValuePair<string, TimeSpan>> Time { get; init; }

        public TotalCategoryTime()
        {
            Time = Array.Empty<KeyValuePair<string, TimeSpan>>();
        }
    }
}