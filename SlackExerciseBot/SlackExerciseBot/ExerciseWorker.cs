namespace SlackExerciseBot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class ExerciseWorker
    {
        private static readonly Random Random = new Random(DateTime.UtcNow.Second);
        private readonly IList<string> exercises = new List<string> { "pushups", "plank hold", "squats" };

        public string SelectExercise()
        {
            var exercise = this.exercises.ElementAt(Random.Next(this.exercises.Count()));
            var reps = Random.Next(20, 35);

            return $"{reps} (seconds or reps) of {exercise}";
        }
    }
}
