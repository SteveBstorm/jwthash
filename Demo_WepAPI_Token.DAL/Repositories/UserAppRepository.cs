using Demo_WepAPI_Token.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Tools;

namespace Demo_WepAPI_Token.DAL.Repositories
{
    public class UserAppRepository : RepositoryBase<Guid, UserAppEntity>
    {
        public UserAppRepository()
            : base("V_UserApp", "Id")
        { }

        protected override UserAppEntity Convert(IDataRecord reader)
        {
            return new UserAppEntity()
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                Email = reader["Email"].ToString(),
                Username = reader["Username"].ToString(),
                Password = null,
                IsAdmin = (bool)reader["IsAdmin"]
            };
        }

        public UserAppEntity Login(string email, string password)
        {
            Command cmd = new Command("LoginUser", true);
            cmd.AddParameter("@Email", email);
            cmd.AddParameter("@Password", password);

            return Connection.ExecuteReader(cmd, Convert).SingleOrDefault();
        }

        public override Guid Insert(UserAppEntity entity)
        {
            Command cmd = new Command("AddUser", true);
            cmd.AddParameter("@email", entity.Email);
            cmd.AddParameter("@username", entity.Username);
            cmd.AddParameter("@password", entity.Password);
            cmd.AddParameter("@isAdmin", 0);

            return (Guid)Connection.ExecuteScalar(cmd);
        }

        public override bool Update(UserAppEntity data)
        {
            Command cmd = new Command("SwitchRoleUser", true);
            cmd.AddParameter("@Id", data.Id);

            return Connection.ExecuteNonQuery(cmd) == 1;

        }

        public override bool Delete(Guid id)
        {
            Command cmd = new Command("DeleteUser", true);
            cmd.AddParameter("@Id", id);

            return Connection.ExecuteNonQuery(cmd) == 1;
        }
    }
}
