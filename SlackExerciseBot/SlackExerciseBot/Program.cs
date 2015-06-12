namespace SlackExerciseBot
{
    using System;
    using System.Threading;

    public class Program
    {
        private const string UserTokenString = "USERTOKENSTRING";
        private const string IncomingWebhookUrl = "INCOMINGWEBHOOKURL";
        private static readonly Random Random = new Random(DateTime.UtcNow.Second);
        private static readonly SlackWorker SlackWorker = new SlackWorker(UserTokenString, IncomingWebhookUrl);
        private static readonly ExerciseWorker ExerciseWorker = new ExerciseWorker();

        public static void Main(string[] args)
        {
            while (true)
            {
                var user = SlackWorker.SelectUserAsync().Result;
                var exercise = ExerciseWorker.SelectExercise();
                var result =
                    $"@{user.name} is the lucky winner of ExerciseSlackBot's lottery.  Your winnings are {exercise}.";

                SlackWorker.NotifyWinnerAsync(result).Wait();

                var sleepPeriod = Random.Next(1800000, 3600000);
                var lottery = $"Next lottery draw is in {Math.Ceiling(sleepPeriod * 0.00001666666)} minutes, good luck!";

                SlackWorker.NotifyWinnerAsync(lottery).Wait();

                Thread.Sleep(sleepPeriod);
            }
        }
    }
}
