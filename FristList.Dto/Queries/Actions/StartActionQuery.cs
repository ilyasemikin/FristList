using System;
using System.Collections.Generic;

namespace FristList.Dto.Queries.Actions
{
    public class StartActionQuery
    {
        public IEnumerable<int> CategoryIds { get; set; }

        public StartActionQuery()
        {
            CategoryIds = Array.Empty<int>();
        }
    }
}