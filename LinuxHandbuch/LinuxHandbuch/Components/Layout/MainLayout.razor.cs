namespace LinuxHandbuch.Components.Layout;

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

partial class MainLayout
{

    public async Task GetFromGpt()
    {
        string apiKey = "sk-proj-wU4mGSu94POOVYiRsIhkT3BlbkFJYJFBmrDkVGitCLf7lJVe";
        string url = "https://api.openai.com/v1/chat/completions";

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "gpt-3.5-turbo", // Use the model you have access to
                messages = new object[]
                {
                    new { role = "system", content = "You are a helpful assistant." },
                    new { role = "user", content = "Tell me a joke." }
                },
                max_tokens = 100,
                temperature = 0.7
            };

            var content = new StringContent(JObject.FromObject(requestBody).ToString(), Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                var responseData = JObject.Parse(responseString);
                Console.WriteLine(responseData["choices"][0]["message"]["content"]);
            }
            else
            {
                Console.WriteLine($"Request failed with status code {response.StatusCode}");
                string errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorResponse);
            }
        }
    }
}
