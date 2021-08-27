using Demo_WepAPI_Token.DAL.Entities;
using Demo_WepAPI_Token.DAL.Repositories;
using Demo_WepAPI_Token.Models;
using Demo_WepAPI_Token.TokenJWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Demo_WepAPI_Token.Utils;
using System.Linq;

namespace Demo_WepAPI_Token.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private UserAppRepository UserAppRepository { get; }
        private MessageRepository MessageRepository { get; }
        private TokenManager TokenManager { get; }

        public UserController(UserAppRepository userAppRepository, TokenManager tokenManager, MessageRepository messageRepository)
        {
            UserAppRepository = userAppRepository;
            TokenManager = tokenManager;
            MessageRepository = messageRepository;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserRegister userRegister)
        {
            if (userRegister is null || !ModelState.IsValid)
                return BadRequest();

            Guid id = UserAppRepository.Insert(new DAL.Entities.UserAppEntity()
            {
                Email = userRegister.Email,
                Username = userRegister.Username,
                Password = userRegister.Password
            });

            // Generate Token
            return Ok(new
            {
                //token = TokenManager.GenerateJWT(id, userRegister.Email)
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Permet de se loguer</remarks>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserLogin userLogin)
        {
            if (userLogin is null || !ModelState.IsValid)
                return BadRequest();

            UserAppEntity userApp = UserAppRepository.Login(userLogin.Email, userLogin.Password);

            if (userApp is null)
                return new ForbidResult();

            // Generate Token
            return Ok(TokenManager.GenerateJWT(userApp));
            
        }

        [HttpGet]
        //[Authorize("admin")]
        public IActionResult GetAll()
        {
            return Ok(UserAppRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetFullUser(Guid id)
        {
            //UserWithMessage user = new UserWithMessage();
            //UserAppEntity userFromDal =  UserAppRepository.Get(id);

            UserWithMessage user = UserAppRepository.Get(id).toAPI();
            user.Messages = MessageRepository.GetAll().Where(u => u.UserId == id).Select(x => x.toAPI());

            return Ok(user);
        }

        [HttpDelete("{Id}")]
        [Authorize("admin")]
        public IActionResult Delete(Guid Id)
        {
            if (UserAppRepository.Get(Id) == null) return BadRequest();

            return Ok(UserAppRepository.Delete(Id));
        }

        [HttpPut]
        [Authorize("admin")]
        public IActionResult SwitchRole(UserAppEntity user)
        {
            return Ok(UserAppRepository.Update(user));
        }


    }
}