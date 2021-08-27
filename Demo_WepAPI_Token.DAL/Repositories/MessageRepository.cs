using Demo_WepAPI_Token.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tools;

namespace Demo_WepAPI_Token.DAL.Repositories
{
    public class MessageRepository : RepositoryBase<Guid, MessageEntity>
    {
        public MessageRepository()
            : base("Message", "Id")
        { }

        protected override MessageEntity Convert(IDataRecord reader)
        {
            return new MessageEntity()
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                Title = reader["Title"].ToString(),
                Content = reader["Content"].ToString(),
                DateMessage = (DateTime)reader["DateMessage"],
                UserId = (reader["UserAppId"] is DBNull) ? null : (Guid?)Guid.Parse(reader["UserAppId"].ToString())
            };
        }

        public override Guid Insert(MessageEntity entity)
        {
            Command cmd = new Command("AddMessage", true);
            cmd.AddParameter("@UserId", entity.UserId);
            cmd.AddParameter("@Title", entity.Title);
            cmd.AddParameter("@Content", entity.Content);

            return (Guid)Connection.ExecuteScalar(cmd);
        }

        public override bool Update(MessageEntity data)
        {
            Command cmd = new Command("UpdateMessage", true);
            cmd.AddParameter("@Id", data.Id);
            cmd.AddParameter("@Title", data.Title);
            cmd.AddParameter("@Content", data.Content);

            return Connection.ExecuteNonQuery(cmd) >= 1;
        }

        public override bool Delete(Guid id)
        {
            Command cmd = new Command("DeleteMessage", true);
            cmd.AddParameter("@Id", id);

            return Connection.ExecuteNonQuery(cmd) >= 1;

        }

       
    }
}
