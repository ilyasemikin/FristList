using System;
using System.Collections.Generic;

namespace FristList.Models
{
    public class RunningAction
    {
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public IReadOnlyList<Category> Categories { get; set; }
    }
}