using System;

namespace FristList.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        
        public int UserId { get; set; }
    }
}