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

            //QuestionLog quesLog = _logDao.PostNewQuestionLog(sessionId, currentQuestion.QuestionId);
            //int quesLogId = quesLog.QuestionLogId;
            //this._questionLogMap.Add(currentIndex, quesLogId);
            //HttpContext.Session.SetInt32("CurrentQuestionIndex", currentIndex);
            return View("QuestionAndOptions", questionLog);

        }

        //public IActionResult NextQuestion()
        //{
        //    currentIndex = HttpContext.Session.GetInt32("CurrentQuestionIndex") ?? 0;
        //    currentIndex++;
        //    if(currentIndex >= _questions.Count )
        //    {
        //        currentIndex = 0;
        //    }
        //    HttpContext.Session.SetInt32("CurrentQuestionIndex", currentIndex);
        //    var currentQuestion = _questions[currentIndex];
        //    return View("QuestionAndOptions", currentQuestion);
        //}
        //public IActionResult PrevQuestion()
        //{
        //    currentIndex--; 
        //    if(currentIndex< 0 ) 
        //    {
        //        currentIndex = 0;
        //    }
        //    var currentQuestion = _questions[currentIndex];
        //    return View("QuestionAndOptions", currentQuestion);
        //}
        [HttpGet("/getoptions/{quesId}")]
        public ActionResult<List<Option>> GetOptionsByQuestionId(int quesId)
        {
            var options = _questionDao.GetOptionsByQuestionId(quesId);
            return Ok(options);
        }

        [HttpPost("[action]")]
        public IActionResult SubmitAnswer(string answer, string sessionId, int quesLogId, int curIndex )
        {
            _questionDao.UpdateQuestionLog(quesLogId, answer);
            var questionLog = _questionDao.GetQuesitonLogBySesssionIdQuesInsessionId(sessionId, curIndex);

            return View("QuestionAndOptions", questionLog);
        }
    }
}
