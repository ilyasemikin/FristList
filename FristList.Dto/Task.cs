using System;
using System.Collections.Generic;

namespace FristList.Dto
{
    public class Task : DtoObjectBase
    {
        public int Id { get; set; }
        public string Name { get; init; }

        public IReadOnlyList<Category> Categories { get; init; }

        public Task()
        {
            Categories = Array.Empty<Category>();
        }
    }
}