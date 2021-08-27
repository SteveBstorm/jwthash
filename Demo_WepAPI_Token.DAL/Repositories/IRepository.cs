using Demo_WepAPI_Token.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WepAPI_Token.DAL.Repositories
{
    public interface IRepository<TKey, TEntity>
        where TEntity : IEntity<TKey>
    {
        // Create
        TKey Insert(TEntity entity);

        // Read
        TEntity Get(TKey id);
        IEnumerable<TEntity> GetAll();

        // Update
        bool Update( TEntity data);

        // Delete
        bool Delete(TKey id);
    }
}
