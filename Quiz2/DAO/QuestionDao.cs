using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Quiz2.Models.DBEntities;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Quiz2.DAO
{
    public class QuestionDao
    {
        private readonly ApplicationDBContext _dbContext;
        public QuestionDao(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Get random questions- options list based on categoryId. 
        public string BuildQuestionsByCategory(int categoryId)
        {
            using(IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                var questions = _dbContext.Questions.Include(q => q.Options).Where(c => c.CategoryId == categoryId).ToList();
                Shuffle(questions);
                var randomQuestions = questions.Take(5).ToList();
                //Create new sessionLog
                string sessionId = PostNewSession(categoryId);
                for (int i = 0; i < randomQuestions.Count; i++)
                {
                    var questionLog = PostNewQuestionLog(sessionId, randomQuestions[i].QuestionId, i + 1);
                }
                transaction.Commit();
                return sessionId;

            }
        }
        public QuestionLog GetQuesitonLogBySesssionIdQuesInsessionId(string sesssionId,int currIndex)
        {
            var questionLog = _dbContext.QuestionLogs.Include(ql =>ql.Question.Options).Where(ql => ql.SessionId == sesssionId && ql.QuesionInSessionId == currIndex)
                .FirstOrDefault();
            //var question = _dbContext.Questions.Find(quesId);
            return questionLog;
        }

        private void Shuffle(List<Question> questions)
        {
            int n = questions.Count;
            while (n > 1)
            {
                n--;
                int k = new Random().Next(n+1);
                Question temp = questions[k];
                questions[k] = questions[n];
                questions[n] = temp;
            }
        }
        public List<Question> GetAllQuestions()
        {
            var questions = _dbContext.Questions.Include(q => q.Options).Include(q => q.Category).ToList();
            return questions;
        }
        public List<Question> GetQuestionsByCategoryId(int categoryId)
        {
            var questions = _dbContext.Questions.
                Include(q => q.Options).Include(q => q.Category).Where(c => c.CategoryId == categoryId).OrderBy(q =>q.QuestionId).ToList();
            return questions;
        }

       
        public List<Option> GetOptionsByQuestionId(int quesId)
        {
            Question question = _dbContext.Questions.Include(q => q.Options).Where(q => q.QuestionId == quesId).FirstOrDefault();
            return question.Options;
        }
        

        public string PostNewSession(int CategoryId)
        {
            SessionLog newSession = new SessionLog(CategoryId);
            _dbContext.SessionLogs.Add(newSession);
            _dbContext.SaveChanges();
            return newSession.SessionId;
        }
        public void UpdateSession(string SessionId, DateTime? EndTime, int? score)
        {
            var session = _dbContext.SessionLogs.Where(s=> s.SessionId == SessionId).FirstOrDefault();
            if (session != null)
            {
                session.EndTime = EndTime ?? session.EndTime;
                session.Score = score ?? session.Score;
            }
            _dbContext.SaveChanges();
        }

        public QuestionLog PostNewQuestionLog(string sessionId, int quesId, int quesionInSessionId)
        {
            QuestionLog newQuestionLog = new QuestionLog(sessionId, quesId, quesionInSessionId);
            _dbContext.QuestionLogs.Add(newQuestionLog);
            _dbContext.SaveChanges();
            return newQuestionLog;
        }
        public void UpdateQuestionLog(int quesLogId, string? answer)
        {
            var questionLog = _dbContext.QuestionLogs.Find(quesLogId);
            if (questionLog != null)
            {
                questionLog.Answer = answer ?? questionLog.Answer;
            }
            _dbContext.SaveChanges();
        }

        public bool IsCompleted(string sessionId)
        {
            var logs = _dbContext.QuestionLogs.Where(ql => ql.SessionId == sessionId).ToList();
            foreach(var log in logs)
            {
                if (log.Answer == null)
                return false;
            }
            return true;
        }
        private bool IsCorrect (string answer, Question question)
        {
            if(answer == null || answer.Length != question.Options.Count) return false;
            bool questRes = true;
            for(int i = 0; i<question.Options.Count; i++)
            {
                bool actual = answer[i] == '1';
                bool expected = question.Options[i].ShouldChoose;
                if(actual != expected) { return false; }
            }
            return true;
        }
        public int GetScore(string sessionId)
        {
            var results = new List<bool>();
            var logs =  _dbContext.QuestionLogs.Include(l => l.Question.Options).Where(ql => ql.SessionId == sessionId).ToList();
            foreach(var log in logs)
            {
                string answer = log.Answer;
                Question question = log.Question;
                results.Add(IsCorrect(answer, question));
            }
            int score = results.Count(item => item);
            return score;
        }

        public SessionLog GetSessionById(string sessionId)
        {
            var session = _dbContext.SessionLogs
                .Where(s => s.SessionId == sessionId).FirstOrDefault();
            return session;
        }
        public Dictionary<int, QuestionResult> GetQuesresultsBySessionId(string sessionId)
        {
            var resultMap = new Dictionary<int, QuestionResult>();  
            var questionLogs = _dbContext.QuestionLogs.
                Where(ql => ql.SessionId == sessionId).ToList();
            foreach(var log in questionLogs)
            {
                var index = log.QuesionInSessionId;
                var question = _dbContext.Questions.Include(q => q.Options).Where(q => q.QuestionId == log.QuestionId).FirstOrDefault();
                string isCorrect = IsCorrect(log.Answer, question) == true ? "Correct" : "Wrong";
                var curQuesRes = new QuestionResult { Answer = log.Answer, Question = question, IsCorrect = isCorrect };
                resultMap.Add(index, curQuesRes);
            }
            return resultMap;
        }
        public Result ReturnResult(string sessionId)
        {
            var sessionlog = GetSessionById(sessionId);
            string passCon =  sessionlog.Score >= 3 ? "QUIZ PASSED" : "QUIZ DID NOT PASS";
            var resultMap = GetQuesresultsBySessionId(sessionId);
            List<int> quesIds=new List<int>();
            foreach(var item in resultMap)
            {
                quesIds.Add(item.Value.Question.QuestionId);
            }
            string categaryName = _dbContext.Categories.Find(sessionlog.CategoryId).CategoryName;
            Result result = new Result
            {
                sessionLog = sessionlog,
                questionResultMap = resultMap,
                PassContiditon = passCon,
                QuizCategory= categaryName,
                QuesId= quesIds,
            };
            return result;
        }

        public List<SessionRow> GetAllSessions()
        {
            var sessionRows = (from sl in _dbContext.SessionLogs
                               join c in _dbContext.Categories
                               on sl.CategoryId equals c.CategoryId
                               select new SessionRow
                               {
                                   TakenDate = sl.StartTime,
                                   Category = c.CategoryName,
                                   NumberOfQuestion = sl.QuestionLogs.Count,
                                   Score = sl.Score,
                                   sessionId = sl.SessionId,
                               }).ToList();
            sessionRows.OrderBy(s=>s.TakenDate).ToList();
            return sessionRows;
        }
        public IEnumerable<SelectListItem> GetCategories()
        {
            var categories = _dbContext.Categories.Select(x => new SelectListItem { Value = x.CategoryId.ToString(), Text = x.CategoryName }).ToList();
            if (categories == null)
                return new List<SelectListItem>();
            return categories;
        }
        public int AddCategory(string categoryName)
        {
            Category category = new Category(categoryName);
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
            return category.CategoryId;
        }
        public int AddQuestion(Question newQues)
        {
            _dbContext.Questions.Add(newQues);
            _dbContext.SaveChanges();
            return newQues.QuestionId;
        }

        public int AddOption(int quesId, string optionValue, bool shouldChoose)
        {
            Option option = new Option(quesId, optionValue, shouldChoose);
            _dbContext.Options.Add(option);
            _dbContext.SaveChanges();
            return option.OptionId;
        }
        
    }
}
