using System;
using System.Collections.Generic;

namespace FristList.Dto.Queries.Import
{
    public class ImportActionsQuery
    {
        public IEnumerable<Action> Actions { get; init; }

        public ImportActionsQuery()
        {
            Actions = Array.Empty<Action>();
        }
    }
}