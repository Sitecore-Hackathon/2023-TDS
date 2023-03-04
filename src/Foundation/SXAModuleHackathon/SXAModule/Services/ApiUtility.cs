using Newtonsoft.Json;
using Sitecore.Shell.Framework.Commands;
using SXAModule.Models;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SXAModule.Services
{
    public class ApiUtility
    {
        public string GetChatMessage(string question)
        {
            string answer;
            CreateStudyNote createStudyNote = new CreateStudyNote()
            {
                model = "text-davinci-003",
                prompt = question,
                temperature = 0.3f,
                max_tokens = 150,
                top_p = 1.0f,
                frequency_penalty = 0.0f,
                presence_penalty = 0.0f
            };

            var jsonCreateStudyNote = JsonConvert.SerializeObject(createStudyNote);
            var client = new HttpClient();
            string chatGPTAPIKey = Sitecore.Configuration.Settings.GetSetting("ChatGPTAPIKey");
            string auth = "Bearer " + chatGPTAPIKey;
            client.DefaultRequestHeaders.Add("Authorization", auth);
            var responseTask = client.PostAsync(Sitecore.Configuration.Settings.GetSetting("ChatGPTRequestURL"),
                new StringContent(jsonCreateStudyNote, Encoding.UTF8, "application/json"));
            var response = responseTask.Result;
            var tokenTask = response.Content.ReadAsStringAsync();
            var token = tokenTask.Result;
            dynamic jsonResult = JsonConvert.DeserializeObject(token);
            answer = Convert.ToString(jsonResult.choices[0].text);
            return answer;
        }
    }
}