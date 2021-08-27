using System;
using System.Collections.Generic;
using System.Text;

namespace Consommation
{
    public class UserToken
    {
        public string Token { get; set; }
    }

    public class Message
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateMessage { get; set; }

        public Guid? UserId { get; set; }
    }

    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }

}
