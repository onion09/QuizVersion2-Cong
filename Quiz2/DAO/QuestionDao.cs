using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Quiz2.Models.DBEntities;

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
        public string GetQuestionsByCategory(int categoryId)
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
            var questionLog = _dbContext.QuestionLogs.Where(ql => ql.SessionId == sesssionId && ql.QuesionInSessionId == currIndex)
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

        public void AddQuestions(int CategoryId, string QuesContent)
        {
            var newQues = new Question { CategoryId = CategoryId, QuesContent = QuesContent };
            _dbContext.Questions.Add(newQues);
            _dbContext.SaveChanges();
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
        public void UpdateSession(int SessionId, DateTime? StartTime, DateTime? EndTime, int? score)
        {
            var session = _dbContext.SessionLogs.Find(SessionId);
            if (session != null)
            {
                session.StartTime = StartTime ?? session.StartTime;
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

    }
}
