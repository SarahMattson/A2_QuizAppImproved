using A2_QuizAppImproved.Models;
using A2_QuizAppImproved.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace A2_QuizAppImproved.Pages;

public class ShowQuiz : PageModel
{
    private readonly IQuizLoader _quizloader;

    public ShowQuiz(IQuizLoader quizLoader)
    {
        _quizloader = quizLoader;
    }

    public Quiz YourQuiz { get; set; } = new Quiz();
    public Boolean DataOk = true;
    public string Error;

    public void OnGet(string selectedQuiz)
    {
        DataOk = true;
        QuizParser(selectedQuiz);
   }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private void QuizParser(string selectedQuiz)
    {
        if (string.IsNullOrEmpty(selectedQuiz))
        {
            DataOk = false;
            Error = "Please select a quiz!";
            return;
        }

        try
        {
            YourQuiz = _quizloader.LoadQuizById(selectedQuiz);

            if (YourQuiz == null)
            {
                DataOk = false;
                Error = "Failed to find your quiz. :(";
            }
        }

        catch (Exception ex)
        {
            DataOk = false;
            Error = $"Cannot load quiz data! {ex.Message}";
        }

    }

    public IActionResult OnPost(string selectedQuiz)
    {
        QuizParser(selectedQuiz);
        string userAnswers = "";

        for (int i = 0; i < YourQuiz.Questions.Count; i++)
        {
            string? selection = Request.Form[$"question{i}"];

            if (string.IsNullOrEmpty(selection))
            {
                Error = "Please answer all the questions. :)";
                DataOk = false;
                return Page();
            }
            userAnswers += selection.ToString() + " q ";
        }

        TempData["sendAnswers"] = userAnswers;
        TempData["userQuiz"] = JsonSerializer.Serialize(YourQuiz).ToString();
        return RedirectToPage("./Results");
    }
}
