using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Models;
using TestMVC.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace TestMVC.Controllers
{
    [EnableCors("MyPolicy")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        private readonly ILogger<UserController> _logger;

        private readonly IActionContextAccessor _accessor;

        public UserController(ILogger<UserController> logger, UserService userService, IActionContextAccessor accessor)
        {
            _logger = logger;
            _userService = userService;
            _accessor = accessor;
        }

        //Register Methods
        [Route("/[controller]/RegisterNew")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("/[controller]/Register")]
        public ActionResult<User> registerUser(User json)
        {
            User user = _userService.create(json);

            return user;
        }

        //Login Methods
        [Route("/[controller]/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("/[controller]/Login/checkUser")]
        public ActionResult<string> checkUser(string email)
        {
            User user = _userService.getBy("email", email);

            if(user.id == null)
            {
                return "not_found";
            }
            else
            {
                return user.image_src;
            }
        }

        [Route("/[controller]/Login/login")]
        public ActionResult<bool> login(string email, string password)
        {
            User user = _userService.getBy("email", email);

            bool check = _userService.checkPasswordMatch(password, user.password);

            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();

            if (check)
            {
                _userService.openSession(user, ip);
                HttpContext.Session.SetString("user_id", user.id);
                HttpContext.Session.SetString("session_time", DateTime.Now.ToString());
            }

            return check;
        }

        [Route("/[controller]/Logout")]
        public void logout()
        {
            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            _userService.closeSession(ip);

            HttpContext.Session.SetString("user_id", "");
            HttpContext.Session.SetString("session_time", "");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
