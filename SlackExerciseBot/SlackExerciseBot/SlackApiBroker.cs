namespace SlackExerciseBot
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public class SlackApiBroker
    {
        private readonly string userTokenString;
        private readonly string incomingWebHookUrl;

        public SlackApiBroker(string userTokenString, string incomingWebHookUrl)
        {
            this.userTokenString = userTokenString;
            this.incomingWebHookUrl = incomingWebHookUrl;
        }

        public async Task<UserListResponse> GetUserListAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var result =
                    await
                    httpClient.GetStringAsync(new Uri($"https://slack.com/api/users.list?token={this.userTokenString}"))
                        .ConfigureAwait(false);
                return JsonConvert.DeserializeObject<UserListResponse>(result);
            }
        }

        public async Task<bool> IsUserActiveAsync(string userId)
        {
            using (var httpClient = new HttpClient())
            {
                var result =
                    await
                    httpClient.GetStringAsync(
                        new Uri($"https://slack.com/api/users.getPresence?token={this.userTokenString}&user={userId}"))
                        .ConfigureAwait(false);
                return JsonConvert.DeserializeObject<PresenceResponse>(result).presence == "active";
            }
        }

        public async Task PushToChannelAsync(string channel, string text)
        {
            using (var httpClient = new HttpClient())
            {
                var webHookRequest = new WebhookRequest
                                         {
                                             channel = channel,
                                             attachments =
                                                 new[]
                                                     { new Attachment { fallback = text, pretext = text } }
                                         };

                var content = new StringContent(
                    JsonConvert.SerializeObject(webHookRequest),
                    Encoding.UTF8,
                    "application/json");
                await httpClient.PostAsync(this.incomingWebHookUrl, content).ConfigureAwait(false);
            }
        }
    }
}
