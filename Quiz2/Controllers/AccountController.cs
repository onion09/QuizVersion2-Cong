using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Quiz2.Models.DBEntities;
using QuizProject.Dao;
using System.Net;
using System.Security.Claims;

namespace CookieAuthenticationExample.Controllers
{

    public class AccountController : Controller
    {
        private readonly AccountDao _accountDao;
        private readonly IMemoryCache _cache;

        public AccountController(AccountDao accountDao, IMemoryCache memoryCache)
        {
            _accountDao = accountDao;
            _cache = memoryCache;

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {

            if (!ModelState.IsValid) return View();
            //verify using check method
            bool isVerified = _accountDao.AuthenticationCheck(username, password);
            if (isVerified)
            {
                var account = _accountDao.GetAccountByUserNamePassowrd(username, password);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim("password", password),
                    new Claim("UserId", account.userId.ToString()),
                    new Claim("Role",account.role)
                };
                var identity = new ClaimsIdentity(claims, "MyCookie");
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                _cache.Set("userId",account.userId.ToString());
                _cache.Set("username", account.username.ToString());

                //Use SignIn method in HttpContext. 
                await HttpContext.SignInAsync("MyCookie", principal);
                return Redirect("/Home/Index");
            }
            return View("Register");
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookie");
            return Redirect("/Account/Login");
        }

        [HttpPost]
        public async Task<IActionResult> Register(Account newAccount)
        {
            
            _accountDao.AddNewAccount(newAccount);

            return Redirect("/Account/Login");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {


            return View("Register");
        }
        [HttpGet]
        [Route("AccountController/GoFeedbackView")]

        public IActionResult GoFeedbackView()
        {
            return View("Feedback");
        }
        [HttpPost]
        public async Task<IActionResult> SubmitFeedback(string feedback)
        {

            _accountDao.SubmitFeedback(feedback);

            return View("Index");
        }
        [HttpGet]
        public async Task<IActionResult> SubmitFeedback()
        {


            return View();
        }
        public async Task<IActionResult> ContactUs()
        {


            return View("Contact");
        }
    }
}
