using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WepAPI_Token.DAL.Entities
{
    public class UserAppEntity : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
