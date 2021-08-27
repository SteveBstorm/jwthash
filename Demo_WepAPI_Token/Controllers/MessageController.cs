using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Demo_WepAPI_Token.DAL.Entities;
using Demo_WepAPI_Token.DAL.Repositories;
using Demo_WepAPI_Token.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo_WepAPI_Token.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private MessageRepository MessageRepository { get; }

        public MessageController(MessageRepository messageRepository)
        {
            this.MessageRepository = messageRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<MessageEntity> messages = MessageRepository.GetAll();

            return Ok(messages);
        }

        [HttpGet("{Id}")]
        [Authorize("user")]
        public IActionResult GetById(Guid Id)
        {
            return Ok(MessageRepository.Get(Id));
        }

        [HttpPost]
        [Authorize("user")]
        public IActionResult AddMessage(Message message)
        {
            if (message is null || !ModelState.IsValid)
                return BadRequest();

            // Recup de l'id dans le token
            ClaimsPrincipal cp = HttpContext.User;
            string id = cp.Claims.SingleOrDefault(c => c.Type == "UserId")?.Value;

            MessageRepository.Insert(new MessageEntity()
            {
                Title = message.Title,
                Content = message.Content,
                UserId = Guid.Parse(id) // <= Info dans le token
            });

            return Ok(); // statut 204
        }

        [HttpDelete("{Id}")]
        [Authorize("admin")]
        public IActionResult Delete(Guid Id)
        {
            if (MessageRepository.Get(Id) == null)
                return BadRequest();

            return Ok(MessageRepository.Delete(Id));
        }

        [HttpPut]
        [Authorize("user")]
        public IActionResult Update(MessageEntity m)
        {
            if (MessageRepository.Get(m.Id) == null)
                return BadRequest();

            return Ok(MessageRepository.Update(m));
        }
    }
}