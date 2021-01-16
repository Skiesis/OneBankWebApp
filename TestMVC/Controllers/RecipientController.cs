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
    [Route("/[controller]")]
    public class RecipientController : Controller
    {
        private readonly RecipientService _recipentService;

        private readonly UserService _userService;

        private readonly BankingService _bankingService;

        public RecipientController(RecipientService recipientService, UserService userService, BankingService bankingService)
        {
            _recipentService = recipientService;
            _userService = userService;
            _bankingService = bankingService;
        }

        [HttpGet]
        [Route("/Banking/Recipient")]
        public IActionResult Recipient()
        {
            User user = _userService.getBy("id", HttpContext.Session.GetString("user_id"));
            string session_time = HttpContext.Session.GetString("session_time");

            ViewBag.user = user;
            ViewBag.session_time = session_time;

            return View();
        }

        [HttpGet]
        public ActionResult<List<Recipient>> Get()
        {
            User user = _userService.getBy("id", HttpContext.Session.GetString("user_id"));
            return _recipentService.Get(user.tax_id);
        }

        [HttpGet]
        [Route("/[controller]/getRecipient")]
        public ActionResult<Recipient> Get(string id)
        {
            var recipient = _recipentService.getBy("id", id);

            if (recipient == null)
            {
                return NotFound();
            }

            return recipient;
        }


        [HttpPost]
        [Route("/[controller]/getUser")]
        public ActionResult<User> getUser(string tax_id)
        {
            var user = _userService.getBy("tax_id", tax_id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        [Route("/[controller]/confirmAccount")]
        public ActionResult<Product> confirmAccount(string recipient, string account)
        {
            var product = _bankingService.getProductByOf("acc_number", account, recipient);

            return product;
        }

        [HttpPost]
        public ActionResult<Recipient> Create(Recipient recipient, string id)
        {
            _recipentService.Create(recipient, id);

            return _recipentService.getBy("id", recipient.id);
        }

        [HttpPut]
        public IActionResult Update(Recipient recipientIn)
        {
            var recipient = _recipentService.getBy("id", recipientIn.id);

            if (recipient == null)
            {
                return NotFound();
            }

            _recipentService.Update(recipientIn);

            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var recipient = _recipentService.getBy("id", id);

            if (recipient == null)
            {
                return NotFound();
            }

            _recipentService.Remove(recipient.id);

            return NoContent();
        }
    }
}