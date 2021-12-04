using System;

namespace FristList.Dto.Responses.Statistics
{
    public class TotalActionTime : DtoObjectBase
    {
        public DateTime From { get; init; }
        public DateTime To { get; init; }
        
        public TimeSpan TotalTime { get; init; }
    }
}