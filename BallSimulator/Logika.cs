using BallSimulator;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Windows;

namespace BallSimulator
{
    public interface IBallService
    {
        void InitializeBalls(int count, int gameWidth, int gameHeight);
        Task MoveBallsAsync();
        IEnumerable<Model> GetBalls();
    }

    public class BallService : IBallService
    {
        private List<Model> balls = new List<Model>();
        private Random random = new Random();
        private int gameWidth, gameHeight;

        public BallService(int gameWidth, int gameHeight)
        {
            this.gameWidth = gameWidth;
            this.gameHeight = gameHeight;
        }

        private bool AreColliding(Model ball1, Model ball2)
        {
            float dx = ball2.X - ball1.X;
            float dy = ball2.Y - ball1.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            return distance <= (ball1.Diameter / 2 + ball2.Diameter / 2);
        }

        private void ResolveCollision(Model ball1, Model ball2)
        {
            lock (ball1)
            {
                lock (ball2)
                {
                    float dx = ball2.X - ball1.X;
                    float dy = ball2.Y - ball1.Y;
                    float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                    if (distance == 0) return;

                    float nx = dx / distance;
                    float ny = dy / distance;

                    float v1n = ball1.VelocityX * nx + ball1.VelocityY * ny;
                    float v1t = -ball1.VelocityX * ny + ball1.VelocityY * nx;

                    float v2n = ball2.VelocityX * nx + ball2.VelocityY * ny;
                    float v2t = -ball2.VelocityX * ny + ball2.VelocityY * nx;

                    float v1nAfter = (v1n * (ball1.Mass - ball2.Mass) + 2 * ball2.Mass * v2n) / (ball1.Mass + ball2.Mass);
                    float v2nAfter = (v2n * (ball2.Mass - ball1.Mass) + 2 * ball1.Mass * v1n) / (ball1.Mass + ball2.Mass);

                    ball1.VelocityX = v1nAfter * nx - v1t * ny;
                    ball1.VelocityY = v1nAfter * ny + v1t * nx;
                    ball2.VelocityX = v2nAfter * nx - v2t * ny;
                    ball2.VelocityY = v2nAfter * ny + v2t * nx;
                }
            }
        }

        private void CheckCollisionWithWalls(Model ball)
        {
            lock (ball)
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
        }

        public void InitializeBalls(int count, int gameWidth, int gameHeight)
        {
            this.gameWidth = gameWidth;
            this.gameHeight = gameHeight;
            balls.Clear();
            for (int i = 0; i < count; i++)
            {
                float mass = GenerateRandomMass();
                balls.Add(new Model
                {
                    X = random.Next(10, gameWidth - 10),
                    Y = random.Next(10, gameHeight - 10),
                    VelocityX = GenerateRandomVelocity(),
                    VelocityY = GenerateRandomVelocity(),
                    Diameter = 10,
                    Mass = mass,
                    GameWidth = gameWidth,
                    GameHeight = gameHeight
                });
            }
        }

        private float GenerateRandomVelocity()
        {
            return (float)(random.NextDouble() * 1 + 1);
        }

        private float GenerateRandomMass()
        {
            return random.Next(2) + 1;
        }

        public async Task MoveBallsAsync()
        {
            const float speedFactor = 0.9f; // Współczynnik skalowania prędkości

            QuadTree quadTree = new QuadTree(0, new Rect(0, 0, gameWidth, gameHeight));
            foreach (var ball in balls)
            {
                quadTree.insert(ball);
            }

            var tasks = balls.Select(async ball =>
            {
                List<Model> returnObjects = new List<Model>();
                quadTree.retrieve(returnObjects, ball);

                foreach (var otherBall in returnObjects)
                {
                    if (ball != otherBall && AreColliding(ball, otherBall))
                    {
                        ResolveCollision(ball, otherBall);
                    }
                }

                lock (ball)
                {
                    ball.X += ball.VelocityX * speedFactor;
                    ball.Y += ball.VelocityY * speedFactor;
                    CheckCollisionWithWalls(ball);
                }
            });

            await Task.WhenAll(tasks);
        }

        public IEnumerable<Model> GetBalls()
        {
            return balls;
        }
    }
}
