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
        public IActionResult AdminIndex(int categoryId)
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

    }
}
