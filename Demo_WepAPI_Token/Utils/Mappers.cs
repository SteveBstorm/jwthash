using Demo_WepAPI_Token.DAL.Entities;
using Demo_WepAPI_Token.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_WepAPI_Token.Utils
{
    public static class Mappers
    {
        public static UserWithMessage toAPI(this UserAppEntity user)
        {
            return new UserWithMessage
            {
                Id = user.Id,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                Username = user.Username
            };
        }

        public static CompleteMessage toAPI(this MessageEntity message) 
        {
            return new CompleteMessage
            {
                Id = message.Id,
                Content = message.Content,
                Title = message.Title,
                DateMessage = message.DateMessage
            };
        } 
    }
}
