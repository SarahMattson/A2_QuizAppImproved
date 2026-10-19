using A2_QuizAppImproved.Models;

namespace A2_QuizAppImproved.Services
{
    public interface IQuizLoader
    {
        IEnumerable<QuizInfo> GetAvailableQuizzes();
        Quiz LoadQuizById(string id);

    }
}
