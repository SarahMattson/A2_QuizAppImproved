using A2_QuizAppImproved.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Use quizzes stored in JSON files
builder.Services.AddScoped<IQuizLoader, JsonFileQuizLoader>();

// Use quizzes retrieved from web service
//builder.Services.AddHttpClient();
//builder.Services.AddScoped<IQuizLoader, HttpQuizLoader>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
