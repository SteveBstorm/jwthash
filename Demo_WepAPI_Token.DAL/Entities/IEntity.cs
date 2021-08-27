using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WepAPI_Token.DAL.Entities
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
