using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace OpenAiConnector
{
    public class OpenAiService
    {
        private string _openAiToken = "";
        public async Task<string> ChatCompletion(dynamic messages)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openAiToken}");

            var requestBody = new
            {
                model = "gpt-4-turbo-preview",
                messages = messages,
                temperature = 1,
                max_tokens = 2000,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", data);
            var result = await response.Content.ReadAsStringAsync();
            var parsedResult = JObject.Parse(result);
            var message = parsedResult["choices"]?[0]?["message"]?["content"]?.ToString();

            return message;
        }
        public object[] BuildMessages(List<string> messages)
        {
            return messages.Select((message, index) => new
            {
                role = index == 0 ? "system" : index % 2 == 0 ? "assistant" : "user",
                content = message
            }).ToArray();
        }
    }
}