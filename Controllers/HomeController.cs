using CorePersonal.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;

namespace CorePersonal.Controllers
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            //Config File 
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Skills()
        {
            return View();
        }

        public IActionResult Resume()
        {
            return View();
        }

        public IActionResult Portfolio()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        #region Contact System
        [HttpPost]
        public IActionResult Contact(ContactViewModel cvm)
         {
             if (!ModelState.IsValid)
            {
                 return View(cvm);
            }
            var mm = new MimeMessage(); //Empty Contructor. 

             string message = $"You have received a new email from your site's contact form!<br/>" +
                $"Sender: {cvm.Name}<br/>Email: {cvm.Email}<br/>Subject: {cvm.Subject}<br/>" +
                $"Message: {cvm.Message}";

             

             mm.From.Add(new MailboxAddress("Sender", _config.GetValue<string>("Credentials:Email:User")));

             mm.To.Add(new MailboxAddress("Personal", _config.GetValue<string>("Credentials:Email:Recipient")));

             mm.Subject = cvm.Subject;

             mm.Body = new TextPart("HTML") { Text = message };
 
            using (var client = new SmtpClient())
            {
                var ClientString = _config.GetValue<string>("Credentials:Email:Client");
                client.Connect(_config.GetValue<string>("Credentials:Email:Client"), 8889);

                //Log-IN
                client.Authenticate(
                    //UserName
                    _config.GetValue<string>("Credentials:Email:User"),
                    //Password
                    _config.GetValue<string>("Credentials:Email:Password")
                    );
                 
                try
                {
                    client.Send(mm);
                }
                catch (Exception ex)
                {
                     ViewBag.ErrorMessage = $"There was an error processing your request." +
                        $"Please try again later <br/>" +
                        $"Error Message: {ex.StackTrace}";

                     return View(cvm);
                }
            }

             return View("EmailConfirmation", cvm);
        }

        #endregion

        [HttpPost]
        private string GetDebuggerDisplay()
        {
            return ToString();
        }


    }
}