using Microsoft.AspNetCore.Mvc;
using Quiz2.DAO;
using Quiz2.Models.DBEntities;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;

namespace Quiz2.Controllers
{
    [Route("AdminController")]
    public class AdminController : Controller

    {
        private readonly QuestionDao _questionDao;
        private readonly LogDao _logDao;


        public AdminController(QuestionDao questionDao, LogDao questionLogDao)
        {
            this._questionDao = questionDao;
            this._logDao = questionLogDao;
        }

        [HttpGet("[action]")]
        public IActionResult AdminIndex()
        {
            
            return View("AdminIndex");
        }
        private void LoadViewBag(string sessionId)
        {
            ViewBag.Finished = _questionDao.IsCompleted(sessionId) ? "1" : "0";
            ViewBag.SessionId = sessionId;
        }

        [HttpGet("[action]")]
        public IActionResult GetAllQuizs()
        {
            var allQuizs = _questionDao.GetAllSessions();
            return View("AllSessions", allQuizs);
        }

        [HttpGet("[action]")]
        public IActionResult DisplayQuizResult(string sessionId)
        {
            Result result = _questionDao.ReturnResult(sessionId);
            return View("DisplayQuizResult", result);
        }

        [HttpGet("[action]")]
        public IActionResult GetAlQuestions()
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
        [HttpGet("[action]")]
        public IActionResult EditQuestion()
        {
            ViewData["Categories"] = _questionDao.GetCategories();
            return View();
        }
    }
}
