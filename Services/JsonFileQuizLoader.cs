using A2_QuizAppImproved.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace A2_QuizAppImproved.Services
{
    public class JsonFileQuizLoader : IQuizLoader
    {
        private readonly string _dataPath; // where the JSON quiz files live
        private readonly string _manifestPath; // the quiz manifest

        public JsonFileQuizLoader(IWebHostEnvironment env)
        {
            _dataPath = Path.Combine(env.ContentRootPath, "AppData");
            _manifestPath = Path.Combine(_dataPath, "JsonFileQuizManifest.json");
        }

        // Read and deserialize the local manifest
        private List<JsonFileQuizInfo> ReadManifest()
        {
            List<JsonFileQuizInfo> manifest; // this is just a box for the jsonfiledata

            string json = File.ReadAllText(_manifestPath); // makes a json string form the files manifest  

            manifest = JsonSerializer.Deserialize<List<JsonFileQuizInfo>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<JsonFileQuizInfo>();
            
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
            {    
                throw new ArgumentException("Quiz ID is required");
            }
            JsonFileQuizInfo info = ReadManifest().FirstOrDefault(q => q.Id == id) as JsonFileQuizInfo;

            if (info == null)
            {
                throw new ArgumentException("Error!");
            }
            // Read the quiz file and convert into a Quiz object.
            string path = Path.Combine(_dataPath, info.FileName);
            string jsonString = File.ReadAllText(path);
            Quiz quiz =  JsonSerializer.Deserialize<Quiz>(
                jsonString, 
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (quiz == null)
            {
                throw new InvalidOperationException("Invalid quiz JSON.");
            } 
            return quiz;
        } 
    }
}