using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BallSimulator
{
    public interface IDataService
    {
        IEnumerable<Model> GetBalls();
        void AddBall(Model ball);
        void ClearBalls();
        bool RemoveBall(Model ball);
        void LogDiagnostics(string filePath);
    }

    public class Data : IDataService
    {
        private ConcurrentBag<Model> balls = new ConcurrentBag<Model>();
        private readonly object _lock = new object();
        public int GameWidth { get; set; }
        public int GameHeight { get; set; }

        public IEnumerable<Model> GetBalls() => balls;

        public void AddBall(Model ball)
        {
            lock (_lock)
            {
                ball.GameWidth = this.GameWidth;
                ball.GameHeight = this.GameHeight;
                balls.Add(ball);
            }
        }

        public bool RemoveBall(Model ball)
        {
            lock (_lock)
            {
                var newBalls = new ConcurrentBag<Model>(balls.Except(new[] { ball }));
                var removed = newBalls.Count < balls.Count;
                balls = newBalls;
                return removed;
            }
        }

        public void ClearBalls()
        {
            lock (_lock)
            {
                balls = new ConcurrentBag<Model>();
            }
        }

        public void LogDiagnostics(string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(balls, options);
            File.WriteAllText(filePath, json);
        }

        // Dowód, że logowanie diagnostyczne nie wpływa na zachowanie piłek
        public void ProveLoggingDoesNotAffectBehavior()
        {
            var initialBalls = balls.ToList();
            LogDiagnostics("diagnostics_test.json");
            var afterLoggingBalls = balls.ToList();

            if (!initialBalls.SequenceEqual(afterLoggingBalls))
            {
                throw new InvalidOperationException("Logging affected ball behavior");
            }
        }
    }
}

