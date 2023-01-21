namespace Quiz2.Models.DBEntities
{
    public class Result
    {
        public SessionLog sessionLog { get; set; }
        public Dictionary<int,QuestionResult>? questionResultMap { get; set; }
        public string? PassContiditon { get; set; }
        public string QuizCategory { get; set; }
        public List<int> QuesId { get; set; }
    }
    public class QuestionResult
    {
        public Question Question { get; set; }
        public string Answer { get; set; }
        public string IsCorrect { get; set; }
    }

    public class SessionRow
    {
        public DateTime TakenDate { get; set; }
        public string Category { get; set; }
        public string UserFullName { get; set; }
        public int NumberOfQuestion { get; set; }
        public int Score { get; set; }
        public string sessionId { get; set; }
    }
}
