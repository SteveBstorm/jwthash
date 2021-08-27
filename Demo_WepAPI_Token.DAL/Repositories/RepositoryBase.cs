using Demo_WepAPI_Token.DAL.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Tools;

namespace Demo_WepAPI_Token.DAL.Repositories
{
    public abstract class RepositoryBase<TKey, TEntity> : IRepository<TKey, TEntity>
        where TEntity : IEntity<TKey>
    {
        protected Connection Connection { get; }
        protected string TableName { get; }
        protected string IdName { get; }

        public RepositoryBase(string tableName, string idName = "Id")
        {
           
            Connection = new Connection(SqlClientFactory.Instance, @"Data Source=DESKTOP-RGPQP6I\TFTIC2014;Initial Catalog=DemoToken;Integrated Security=True;");

            TableName = tableName;
            IdName = idName;
        }

        protected abstract TEntity Convert(IDataRecord reader);

        public virtual IEnumerable<TEntity> GetAll()
        {
            Command cmd = new Command("SELECT * FROM [" + TableName + "]");

            return Connection.ExecuteReader(cmd, Convert);
        }
        public virtual TEntity Get(TKey id)
        {
            Command cmd = new Command("SELECT * FROM [" + TableName + "] " +
                                      "WHERE " + IdName + " = @Id");
            cmd.AddParameter("@Id", id);

            return Connection.ExecuteReader(cmd, Convert).SingleOrDefault();
        }


        public abstract TKey Insert(TEntity entity);
        public abstract bool Update(TEntity data);
        public abstract bool Delete(TKey id);
    }
}
