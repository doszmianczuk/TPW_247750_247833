using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallSimulator
{
    public class Data
    {
        private List<Model> balls = new List<Model>();

        public IEnumerable<Model> GetBalls() => balls;

        public void AddBall(Model ball)
        {
            balls.Add(ball);
        }

        public void ClearBalls()
        {
            balls.Clear();
        }

        public bool RemoveBall(Model ball)
        {
            return balls.Remove(ball);
        }
    }

}