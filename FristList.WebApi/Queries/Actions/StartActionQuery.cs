using System;
using System.Collections.Generic;

namespace FristList.WebApi.Queries.Actions
{
    public class StartActionQuery
    {
        public IEnumerable<int> CategoryIds { get; set; }
    }
}