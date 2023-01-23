using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Quiz2.Models.DBEntities;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Quiz2.DAO
{
    public class QuestionDao
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMemoryCache _cache;

        public QuestionDao(ApplicationDBContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _cache = memoryCache;
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
                session.UserId = int.Parse(_cache.Get<string>("userId"));
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
            var session = _dbContext.SessionLogs.Include(s=>s.Account)
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
            var userFullName = sessionlog.Account.firstName + " " +sessionlog.Account.lastName;
            Result result = new Result
            {   UserFullName= userFullName,
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

        public int AddOption(Option newOption)
        {
            _dbContext.Options.Add(newOption);
            _dbContext.SaveChanges();
            return newOption.OptionId;
        }
        public Question GetQuestionById(int questionId)
        {
            var question = _dbContext.Questions.Include(q=>q.Category).Where(q => q.QuestionId== questionId).FirstOrDefault();
            return question;
        }
        public Option GetOptionByQuestionId(int quesId)
        {
            Option option = _dbContext.Options.Include(q => q.Question).ThenInclude(q => q.Options).Where(q => q.QuestionId == quesId).FirstOrDefault();
            return option;
        }
        public Option GetOptionById(int optionId)
        {
            Option option = _dbContext.Options.Include(o=>o.Question).Where(o=>o.OptionId== optionId).FirstOrDefault();
            return option;
        }

        public void UpdateQuestion(int quesId, Question updateQuestion)
        {
            var question = GetQuestionById(quesId);
            if(question!= null)
            {
                question.QuesContent = updateQuestion.QuesContent;
                question.CategoryId = updateQuestion.CategoryId;
                _dbContext.SaveChanges();
            }
        }
        public void UpdateOption(int optionId, Option updateOption)
        {
            var option = GetOptionById(optionId);
            if (option != null)
            {
                option.OptionValue = updateOption.OptionValue;
                option.ShouldChoose = updateOption.ShouldChoose;
                _dbContext.SaveChanges();
            }
        }
        public void DeleteQuestion(int quesId)
        {
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            var question = GetQuestionById(quesId);
            if(question != null)
            {
                var options = _dbContext.Options.Where(o => o.QuestionId == quesId);
                _dbContext.Options.RemoveRange(options);
                var quesLog = _dbContext.QuestionLogs.Where(ql => ql.QuestionId == quesId);
                _dbContext.QuestionLogs.RemoveRange(quesLog);
                _dbContext.Questions.Remove(question);
                _dbContext.SaveChanges();
            }
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        }
        public void DeleteOption(int optionId) 
        { 
            var option = GetOptionById(optionId);
            if(option != null)
            {
                _dbContext.Options.Remove(option);
                _dbContext.SaveChanges();
            }
        }
    }
}
