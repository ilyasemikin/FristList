using System.Collections.Generic;

namespace FristList.WebApi.Queries.Tasks
{
    public class CreateTaskQuery
    {
        public string Name { get; set; }
        public IEnumerable<int> Categories { get; set; }
    }
}