
using BallSimulator;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace BallSimulator
{

    public interface IBallService
    {
        void InitializeBalls(int count, int gameWidth, int gameHeight);
        void MoveBalls();
        IEnumerable<Ball> GetBalls();
    }

    public class BallService : IBallService
    {
        private readonly List<Ball> balls = new List<Ball>();
        private readonly Random random = new Random();
        private readonly int gameWidth;
        private readonly int gameHeight;

        public BallService(int gameWidth, int gameHeight)
        {
            this.gameWidth = gameWidth;
            this.gameHeight = gameHeight;
        }



        private void CheckCollisionWithWalls(Ball ball)
        {
            if (ball.X - ball.Diameter / 2 <= 0 || ball.X + ball.Diameter / 2 >= gameWidth)
            {
                ball.VelocityX = -ball.VelocityX;
            }
            if (ball.Y - ball.Diameter / 2 <= 0 || ball.Y + ball.Diameter / 2 >= gameHeight)
            {
                ball.VelocityY = -ball.VelocityY;
            }
        }


        private float Clamp(float value, float min, float max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }


        public void InitializeBalls(int count, int gameWidth, int gameHeight)
        {
            balls.Clear();
            for (int i = 0; i < count; i++)
            {
                balls.Add(new Ball
                {
                    X = random.Next(10, gameWidth - 10),
                    Y = random.Next(10, gameHeight - 10),
                    VelocityX = GenerateRandomVelocity(),
                    VelocityY = GenerateRandomVelocity(),
                    Diameter = 10
                });
            }
        }

        private float GenerateRandomVelocity()
        {
            // v od -2 do 2
            return (float)(random.NextDouble() * 4 - 2);
        }

        public void MoveBalls()
        {
            foreach (var ball in balls)
            {
                ball.X += ball.VelocityX;
                ball.Y += ball.VelocityY;

                CheckCollisionWithWalls(ball);

            }
        }

        public IEnumerable<Ball> GetBalls()
        {
            return balls;
        }


    }


}