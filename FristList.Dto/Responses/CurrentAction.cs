using System;
using System.Collections.Generic;
using FristList.Dto.Responses.Base;

namespace FristList.Dto.Responses
{
    public class CurrentAction : DtoObjectBase
    {
        public DateTime StartTime { get; init; }
        public IReadOnlyList<Category> Categories { get; init; }

        public CurrentAction()
        {
            Categories = Array.Empty<Category>();
        }
    }
}