using A2_QuizAppImproved.Models;
using System.Text.Json;

namespace A2_QuizAppImproved.Services
{
    public class HttpQuizLoader : IQuizLoader
    {
        private readonly HttpClient _http; // used to retrieve a quiz from the web service
        private readonly string _manifestPath; // the quiz manifest 

        public HttpQuizLoader(HttpClient http, IWebHostEnvironment env)
        {
            _http = http;
            _manifestPath = Path.Combine(
                env.ContentRootPath,
                "AppData",
                "HttpQuizManifest.json"
            );
        }

        // Read and deserialize the local manifest
        private List<HttpQuizInfo> ReadManifest()
        {
            List<HttpQuizInfo> manifest;

            string json = File.ReadAllText(_manifestPath);

            manifest = JsonSerializer.Deserialize<List<HttpQuizInfo>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<HttpQuizInfo>();

            return manifest;
        }

        // Return the available quizzes for the Start page
        public IEnumerable<QuizInfo> GetAvailableQuizzes()
        {
            return ReadManifest();
        }

        // Load a particular quiz by its ID
        public Quiz LoadQuizById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Quiz ID is required.");

            // Find the requested quiz in the manifest
            var info = ReadManifest().FirstOrDefault(q => q.Id == id);

            if (info == null)
                throw new ArgumentException("Invalid quiz ID.");

            // Retrieve the quiz JSON from GitHub Pages
            string json = _http.GetStringAsync(info.Url)
                               .GetAwaiter()
                               .GetResult();

            // Deserialize the quiz
            var quiz = JsonSerializer.Deserialize<Quiz>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (quiz == null)
                throw new InvalidOperationException("Invalid quiz JSON.");

            return quiz;
        }
    }
}