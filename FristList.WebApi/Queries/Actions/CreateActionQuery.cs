using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FristList.WebApi.QueryValidators;

namespace FristList.WebApi.Queries.Actions
{
    public class CreateActionQuery
    {
        [Required]
        [DateTimeLessThan(nameof(EndTime))]
        public DateTime StartTime { get; set; }
        
        [Required]
        public DateTime EndTime { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        public IEnumerable<int> Categories { get; set; }
    }
}