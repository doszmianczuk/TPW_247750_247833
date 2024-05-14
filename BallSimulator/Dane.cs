using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BallSimulator
{
    public interface IDataService
    {
        IEnumerable<Model> GetBalls();
        void AddBall(Model ball);
        void ClearBalls();
        bool RemoveBall(Model ball);
    }

    public class Data : IDataService
    {
        private ConcurrentBag<Model> balls = new ConcurrentBag<Model>();

        public IEnumerable<Model> GetBalls() => balls;

        public void AddBall(Model ball)
        {
            balls.Add(ball);
        }

        public void ClearBalls()
        {
            balls = new ConcurrentBag<Model>(); // Tworzenie nowego ConcurrentBag, aby wyczyścić.
        }

        public bool RemoveBall(Model ball)
        {
            // ConcurrentBag nie ma metody Remove, więc alternatywą jest ponowne stworzenie kolekcji bez elementu.
            var newBalls = new ConcurrentBag<Model>(balls.Except(new[] { ball }));
            var removed = newBalls.Count < balls.Count;
            balls = newBalls;
            return removed;
        }
    }
}
