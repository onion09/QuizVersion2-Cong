using Microsoft.AspNetCore.Mvc;
using Quiz2.DAO;
using Quiz2.Models.DBEntities;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;

namespace Quiz2.Controllers
{
    [Route("QuizController")]
    public class QuestionOptionsController : Controller

    {
        private readonly QuestionDao _questionDao;
        private readonly LogDao _logDao;
        //private int currentIndex;
        //private List<Question> _questions;
        //private int _sessionId;
        //private Dictionary<int, int> _questionLogMap = new Dictionary<int, int>();
        //private readonly ISessionStore _sessionStore;

        public QuestionOptionsController(QuestionDao questionDao, LogDao questionLogDao)
        {
            this._questionDao = questionDao;
            this._logDao = questionLogDao;
        }

        [HttpGet("/getquestions/{categoryId}")]
        public IActionResult FirstQuestion(int categoryId)
        {
            string sessionId = _questionDao.GetQuestionsByCategory(categoryId);
            var questionLog = _questionDao.GetQuesitonLogBySesssionIdQuesInsessionId(sessionId, 1);
            LoadViewBag(sessionId);
            return View("Quiz", questionLog);
        }
        private void LoadViewBag(string sessionId)
        {
            ViewBag.Finished = _questionDao.IsCompleted(sessionId) ? "1" : "0";
            ViewBag.SessionId = sessionId;
        }

        [HttpPost("[action]")]
        public IActionResult DisplayNextQuestion(string sessionId, int quesLogId, int curIndex)
        {
            curIndex++;
            if (curIndex > 5)
            {
                curIndex = 5;
            }
            var currentLog = _questionDao.GetQuesitonLogBySesssionIdQuesInsessionId(sessionId, curIndex);
            LoadViewBag(sessionId);
            return View("Quiz", currentLog);
        }

        [HttpPost("[action]")]
        public IActionResult DisplayPrevQuestion(string sessionId, int quesLogId, int curIndex)
        {
            curIndex--;
            if (curIndex < 1)
            {
                curIndex = 1;
            }
            var currentLog = _questionDao.GetQuesitonLogBySesssionIdQuesInsessionId(sessionId, curIndex);
            this.LoadViewBag(sessionId);
            return View("Quiz", currentLog);
        }
        [HttpGet("/getoptions/{quesId}")]
        public ActionResult<List<Option>> GetOptionsByQuestionId(int quesId)
        {
            var options = _questionDao.GetOptionsByQuestionId(quesId);
            return Ok(options);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitAnswer(string answer, string sessionId, int quesLogId, int curIndex )
        {
            _questionDao.UpdateQuestionLog(quesLogId, answer);
            var questionLog = _questionDao.GetQuesitonLogBySesssionIdQuesInsessionId(sessionId, curIndex);

            return View("Quiz", questionLog);
        }

        [HttpGet("[action]")]
        public IActionResult DisplayQuizResult(string sessionId)
        {
            int score = _questionDao.GetScore(sessionId);
            //string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            _questionDao.UpdateSession(sessionId, DateTime.Now, score);
            Result result = _questionDao.ReturnResult(sessionId);
            return View("DisplayQuizResult", result);
        }

    }
}
