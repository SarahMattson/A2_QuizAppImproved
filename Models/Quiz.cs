namespace A2_QuizAppImproved.Models
{
    public class Quiz
    {
        public string Title { get; set; } = "";
        public List<QuizQuestion> Questions { get; set; } = new();
    }
}
