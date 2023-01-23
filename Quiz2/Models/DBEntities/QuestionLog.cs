using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Quiz2.Models.DBEntities
{
    [Table("QuestionLog")]
    public class QuestionLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionLogId { get; set; }
        public string SessionId { get; set; }
        public int QuestionId { get; set; }
        public int QuesionInSessionId { get; set; }
        public string? Answer { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
        [ForeignKey("SessionId")]
        [JsonIgnore]
        public SessionLog SessionLog { get; set; }
        public QuestionLog() { }    
        public QuestionLog(string sessionId, int quesId, int quesionInSessionId)
        {
            this.SessionId= sessionId;
            this.QuestionId= quesId;
            this.QuesionInSessionId= quesionInSessionId;
        }
    }

    [Table("SessionLog")]
    public class SessionLog
    {
        [Key]
        public string SessionId { get; set; }
        public int CategoryId { get; set; }
        public int? UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Score { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [InverseProperty("SessionLog")]
        public List<QuestionLog> QuestionLogs { get; set; }

        [InverseProperty("SessionLogs")]
        public Account Account { get; set; }
        //[NotMapped] public string QuizName { get; set; }


        public SessionLog(int CategoryId) 
        {
            this.StartTime = DateTime.Now;
            this.SessionId = Guid.NewGuid().ToString();
            this.CategoryId = CategoryId;
        }
        public SessionLog()
        {
            this.StartTime = DateTime.MinValue;
            this.SessionId = Guid.NewGuid().ToString();
            this.CategoryId = 100;
        }
    }
}
