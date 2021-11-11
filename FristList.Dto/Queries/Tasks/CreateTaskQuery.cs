using System.Collections.Generic;

namespace FristList.Dto.Queries.Tasks
{
    public class CreateTaskQuery
    {
        public string Name { get; set; }
        public int? ProjectId { get; set; }
        public IEnumerable<int> Categories { get; set; }
    }
}