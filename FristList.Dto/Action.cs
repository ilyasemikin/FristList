using System;
using System.Collections.Generic;

namespace FristList.Dto
{
    public class Action
    {
        public int Id { get; init; }
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public string Description { get; init; }
        
        public IReadOnlyList<Category> Categories { get; init; }

        public Action()
        {
            Categories = Array.Empty<Category>();
        }
    }
}