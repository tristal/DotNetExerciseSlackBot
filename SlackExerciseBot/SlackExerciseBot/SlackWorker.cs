namespace SlackExerciseBot
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class SlackWorker
    {
        private static readonly Random Random = new Random(DateTime.UtcNow.Second);
        private readonly SlackApiBroker slackApiBroker;

        public SlackWorker(string userTokenString, string incomingWebhookUrl)
        {
            this.slackApiBroker = new SlackApiBroker(userTokenString, incomingWebhookUrl);
        }

        public async Task<Member> SelectUserAsync()
        {
            var users = await this.slackApiBroker.GetUserListAsync().ConfigureAwait(false);

            foreach (var user in users.members)
            {
                user.is_active = await this.slackApiBroker.IsUserActiveAsync(user.id).ConfigureAwait(false);
            }

            var activeUsers = users.members.Where(user => user.is_active).ToList();
            var userIndex = Random.Next(activeUsers.Count());
            var selectedUser = activeUsers.ElementAt(userIndex);

            Console.WriteLine($"User {selectedUser.name}");

            return selectedUser;
        }

        public Task NotifyWinnerAsync(string text)
        {
            return this.slackApiBroker.PushToChannelAsync("#general", text);
        }
    }
}
