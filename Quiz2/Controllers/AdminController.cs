using Microsoft.AspNetCore.Mvc;
using Quiz2.DAO;
using Quiz2.Models.DBEntities;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuizProject.Dao;
using Microsoft.AspNetCore.Authorization;

namespace Quiz2.Controllers
{
    [Route("AdminController")]
    [Authorize]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller

    {
        private readonly QuestionDao _questionDao;
        private readonly AccountDao _accountDao;

        public AdminController(QuestionDao questionDao, AccountDao accountDao)
        {
            this._questionDao = questionDao;
            this._accountDao = accountDao;
        }

        [HttpGet("[action]")]
        public IActionResult AdminIndex()
        {
            
            return View("AdminIndex");
        }


        [HttpGet("[action]")]
        public IActionResult GetAllQuizs()
        {
            var allUsers = _accountDao.GetUserIds();
            ViewData["allUsers"] = allUsers;
            var allQuizs = _questionDao.GetAllSessions();
            return View("AllSessions", allQuizs);
        }

        [HttpGet("[action]")]
        public IActionResult DisplayQuizResult(string sessionId)
        {
            Result result = _questionDao.ReturnResult(sessionId);
            return View("DisplayQuizResult", result);
        }
        [HttpPost("[action]")]
        public IActionResult SelectSessionByUserId(string userId)
        {
            var allUsers = _accountDao.GetUserIds();
            ViewData["allUsers"] = allUsers;
            var quizByUser = _questionDao.GetSessionsByUserId(userId);
            return View("AllSessions", quizByUser);
        }

        [HttpGet("[action]")]
        public virtual IActionResult GetAlQuestions()
        {
            var allQuestions = _questionDao.GetAllQuestions();
            return View("AllQuestions", allQuestions);
        }
        [HttpGet("[action]")]
        public IActionResult GetQuestionsByCategory(int categoryId)
        {
            var quesByCategory = _questionDao.GetQuestionsByCategoryId(categoryId);
            return View("AllQuestions", quesByCategory);
        }

        [HttpPost("[action]")]
        public IActionResult CreateCategory(string categoryName)
        {
            _questionDao.AddCategory(categoryName);
            return RedirectToAction("Index"); //after adding new category go back to index page
        }
        [HttpPost("[action]")]
        public IActionResult CreateQuestion(Question question)
        {
            _questionDao.AddQuestion(question);
            return RedirectToAction("GetAlQuestions"); //after adding new question go back to index page
        }
        [HttpGet("[action]")]
        public IActionResult CreateQuestion()
        {
            ViewData["Categories"] = _questionDao.GetCategories();
            return View();
        }

        [HttpPost("[action]")]
        public IActionResult CreateOption(Option option)
        {
            _questionDao.AddOption(option);
            return RedirectToAction("GetAlQuestions"); //after adding new question go back to index page
        }
        [HttpGet("[action]")]
        public IActionResult CreateOption(int quesId)
        {
            List<SelectListItem> shoudChoose = new List<SelectListItem>() {
            new SelectListItem { Text = "No", Value ="false"},
            new SelectListItem{Text = "Yes", Value ="true"}
            };
            ViewBag.ShoudChoose = shoudChoose;
            ViewData["quesId"] = quesId;
            var option = _questionDao.GetOptionByQuestionId(quesId);
            return View();
        }
        [HttpGet("[action]")]
        public IActionResult EditQuestion(int quesId)
        {
            ViewData["Categories"] = _questionDao.GetCategories();
            var question = _questionDao.GetQuestionById(quesId);
            return View(question);
        }
        [HttpPost("[action]")]
        public IActionResult EditQuestion(int quesId,Question updatedQuestion)
        {
            _questionDao.UpdateQuestion(quesId, updatedQuestion);
            return RedirectToAction("GetAlQuestions");
        }
        [HttpGet("[action]")]
        public IActionResult EditOption(int optionId)
        {
            var option = _questionDao.GetOptionById(optionId);
            List<SelectListItem> shoudChoose = new List<SelectListItem>() {
            new SelectListItem { Text = "No", Value ="false"},
            new SelectListItem{Text = "Yes", Value ="true"}
            };
            ViewBag.ShoudChoose = shoudChoose;
            return View(option);
        }
        [HttpPost("[action]")]
        public IActionResult EditOption(int optionId, Option updatedOption)
        {
            _questionDao.UpdateOption(optionId, updatedOption);
            return RedirectToAction("GetAlQuestions");
        }
        [HttpGet("[action]")]
        public IActionResult DeleteQuestion(int quesId)
        {
            var question = _questionDao.GetQuestionById(quesId);
            return View(question);
        }
        [HttpPost("[action]")]
        public IActionResult DeleteQuestion (int quesId, Question quesDelete)
        {
            _questionDao.DeleteQuestion(quesId);
            return RedirectToAction("GetAlQuestions");
        }
        [HttpGet("[action]")]
        public IActionResult DeleteOption(int optionId)
        {
            var option = _questionDao.GetOptionById(optionId);
            return View(option);
        }
        [HttpPost("[action]")]
        public IActionResult DeleteOption(int optionId, Option option)
        {
            _questionDao.DeleteOption(optionId);
            return RedirectToAction("GetAlQuestions");
        }
        [HttpGet("[action]")]
        public IActionResult GetAllUsers()
        {
            var accounts = _accountDao.GetAllAccounts();
            return View("AllUsers",accounts);  
        }
    }
}
