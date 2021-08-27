using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_WepAPI_Token.Models
{
    public class Message
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Content { get; set; }
    }
}
