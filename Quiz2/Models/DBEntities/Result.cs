namespace Quiz2.Models.DBEntities
{
    public class Result
    {
        public SessionLog sessionLog { get; set; }
        public Dictionary<int,QuestionResult>? questionResultMap { get; set; }
        public string? PassContiditon { get; set; }
        public string QuizCategory { get; set; }
    }
    public class QuestionResult
    {
        public Question Question { get; set; }
        public string Answer { get; set; }
    }

}
