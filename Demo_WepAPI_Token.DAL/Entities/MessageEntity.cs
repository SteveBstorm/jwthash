using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WepAPI_Token.DAL.Entities
{
    public class MessageEntity: IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateMessage { get; set; }

        public Guid? UserId { get; set; }
    }
}
