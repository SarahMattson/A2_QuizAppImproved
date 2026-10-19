namespace A2_QuizAppImproved.Models
{
    public class QuizQuestion
    {
        public string QuestionText { get; set; } = "";
        public List<string> Choices { get; set; } = new();
        public int AnswerIndex { get; set; } = -1;
    }
}
