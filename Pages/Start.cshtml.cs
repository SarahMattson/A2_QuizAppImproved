using A2_QuizAppImproved.Models;
using A2_QuizAppImproved.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace A2_QuizAppImproved.Pages;

public class StartModel : PageModel
{
    private readonly IQuizLoader _quizLoader;

    public StartModel(IQuizLoader quizLoader)
    {
        _quizLoader = quizLoader;
    }

    public Dictionary<string, QuizInfo> Quizzes {get; set;} = new();

    [BindProperty]
    public string ChosenQuizID {get; set;}

    public IActionResult OnGet()
    {
        var quizList = _quizLoader.GetAvailableQuizzes();
        Quizzes = quizList.ToDictionary(q => q.Id);

        return Page();;
    }
    
    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(ChosenQuizID))
        {
            return Page();
        }
        return RedirectToPage("./ShowQuiz", new { selectedQuiz = ChosenQuizID });
    }
}