using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class ArticleController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Summarize(string articleUrl)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://article-extractor-and-summarizer.p.rapidapi.com/summarize?url={articleUrl}&length=3"),
            Headers =
            {
                { "X-RapidAPI-Key", "c4bbb25239msh26bf68efa8534b7p1b768ajsn09c475eb2701" },
                { "X-RapidAPI-Host", "article-extractor-and-summarizer.p.rapidapi.com" },
            },
        };

        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var summaryObject = JsonSerializer.Deserialize<SummaryResponse>(jsonString);

            if (summaryObject != null)
            {
                // Extract the content and replace newline characters with HTML line breaks
                ViewBag.Summary = summaryObject.summary.Replace("\n", "<br>");
            }
        }

        return View("Index");
    }

    // Create a class to represent the structure of the JSON response
    public class SummaryResponse
    {
        public string summary { get; set; }
    }
}
