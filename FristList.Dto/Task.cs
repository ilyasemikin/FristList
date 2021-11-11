using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FristList.Dto
{
    public class Task
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