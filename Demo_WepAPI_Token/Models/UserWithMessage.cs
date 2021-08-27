using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_WepAPI_Token.Models
{
    public class UserWithMessage
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }

        public IEnumerable<CompleteMessage>? Messages { get; set; }
    }
}
