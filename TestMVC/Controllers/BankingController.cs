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
    public class BankingController : Controller
    {
        private readonly BankingService _bankingService;

        private readonly UserService _userService;

        private readonly RecipientService _recipientService;

        private readonly ILogger<BankingController> _logger;

        private readonly IActionContextAccessor _accessor;

        public BankingController(ILogger<BankingController> logger, BankingService bankingService, IActionContextAccessor accessor, UserService userService, RecipientService recipientService)
        {
            _logger = logger;
            _bankingService = bankingService;
            _accessor = accessor;
            _userService = userService;
            _recipientService = recipientService;
        }

        //Landing Page
        [Route("/[controller]/Landing")]
        public IActionResult Landing()
        {
            User user = _userService.getBy("id", HttpContext.Session.GetString("user_id"));
            List<Product> products = _bankingService.getUserProducts(user.id);
            string session_time = HttpContext.Session.GetString("session_time");

            ViewBag.user = user;
            ViewBag.products = products;
            ViewBag.session_time = session_time;

            return View();
        }

        //Consult accounts
        [Route("/[controller]/Consult/{id?}")]
        public IActionResult Consult(string id)
        {
            User user = _userService.getBy("id", HttpContext.Session.GetString("user_id"));
            List<Product> products = _bankingService.getUserProducts(user.id);
            string session_time = HttpContext.Session.GetString("session_time");
            Product product = _bankingService.getProductBy("acc_number", id);

            ViewBag.user = user;
            ViewBag.products = products;
            ViewBag.session_time = session_time;
            ViewBag.product = product;

            return View();
        }

        //Get transactions from account
        [HttpPost]
        [Route("/[controller]/Consult/getTransactions")]
        public ActionResult<List<Transaction>> getTransactions(string product)
        {
            List<Transaction> transactions = _bankingService.getTransactionsOf(product);

            return transactions;
        }

        //Get product by id
        [HttpPost]
        [Route("/[controller]/Consult/getProduct")]
        public ActionResult<Product> getProduct(string product)
        {
            Product product_o = _bankingService.getProductBy("id", product);

            return product_o;
        }

        //Self transactions
        [Route("/[controller]/Transaction/Self")]
        public IActionResult Self()
        {
            User user = _userService.getBy("id", HttpContext.Session.GetString("user_id"));
            List<Product> products = _bankingService.getUserProducts(user.id);
            string session_time = HttpContext.Session.GetString("session_time");

            ViewBag.user = user;
            ViewBag.products = products;
            ViewBag.session_time = session_time;

            return View();
        }

        //Make transaction between self accounts
        [HttpPost]
        [Route("/[controller]/Transaction/SelfTransaction")]
        public void makeSelfTransaction(Transaction transaction)
        {
            _bankingService.makeSelfTransaction(transaction);
        }

        //Others transactions
        [Route("/[controller]/Transaction/Others")]
        public IActionResult Other()
        {
            User user = _userService.getBy("id", HttpContext.Session.GetString("user_id"));
            List<Product> products = _bankingService.getUserProducts(user.id);
            string session_time = HttpContext.Session.GetString("session_time");
            List<Recipient> recipients = _recipientService.Get(user.tax_id);

            ViewBag.user = user;
            ViewBag.products = products;
            ViewBag.session_time = session_time;
            ViewBag.recipients = recipients;

            return View();
        }

        //Make transaction to others accounts
        [HttpPost]
        [Route("/[controller]/Transaction/OtherTransaction")]
        public void makeOtherTransaction(Transaction transaction)
        {
            _bankingService.makeOtherTransaction(transaction);
        }

        //Interbanking transactions
        [Route("/[controller]/Transaction/Interbanking")]
        public IActionResult Interbanking()
        {
            User user = _userService.getBy("id", HttpContext.Session.GetString("user_id"));
            List<Product> products = _bankingService.getUserProducts(user.id);
            string session_time = HttpContext.Session.GetString("session_time");

            ViewBag.user = user;
            ViewBag.products = products;
            ViewBag.session_time = session_time;

            return View();
        }

        //Make transaction interbanking
        [HttpPost]
        [Route("/[controller]/Transaction/InterbankingTransaction")]
        public void makeInterbankingTransaction(Transaction transaction)
        {
            _bankingService.makeInterbankingTransaction(transaction);
        }
    }
}
