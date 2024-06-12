using BallSimulator;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Text.Json;
using System.Timers;

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
        private double _maxWidth = 582;
        private double _maxHeight = 282;
        private readonly Random random = new Random();
        private List<Model> balls = new List<Model>();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private System.Timers.Timer aTimer;
        FileStream filestream;

        public BallService(int gameWidth, int gameHeight)
        {
            this._maxWidth = gameWidth;
            this._maxHeight = gameHeight;
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
                if (ball.X - ball.Diameter / 2 <= 0 || ball.X + ball.Diameter / 2 >= _maxWidth)
                {
                    ball.VelocityX = -ball.VelocityX;
                }
                if (ball.Y - ball.Diameter / 2 <= 0 || ball.Y + ball.Diameter / 2 >= _maxHeight)
                {
                    ball.VelocityY = -ball.VelocityY;
                }
            }
        }

        public void InitializeBalls(int count, int gameWidth, int gameHeight)
        {
            _maxWidth = gameWidth;
            _maxHeight = gameHeight;
            balls.Clear();

            for (int i = 0; i < count; i++)
            {
                Model ball = new Model();
                ball.X = random.Next(10, (int)(_maxWidth));
                ball.Y = random.Next(10, (int)(_maxHeight));
                ball.VelocityX = GenerateRandomVelocity();
                ball.VelocityY = GenerateRandomVelocity();
                ball.Diameter = 10;
                ball.Mass = random.Next(1, 4);
                balls.Add(ball);
            }

            _cancellationTokenSource = new CancellationTokenSource();
            StartLogging("log.json");
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
            await Task.Run(() =>
            {
                foreach (var ball in balls)
                {
                    ball.X += ball.VelocityX;
                    ball.Y += ball.VelocityY;

                    if (ball.X <= 0 || ball.X >= _maxWidth)
                    {
                        ball.VelocityX = -ball.VelocityX;
                    }
                    if (ball.Y <= 0 || ball.Y >= _maxHeight)
                    {
                        ball.VelocityY = -ball.VelocityY;
                    }
                }
            });
        }

        public IEnumerable<Model> GetBalls()
        {
            return balls;
        }

        private void StartLogging(string filePath)
        {
            filestream = File.Create(filePath);

            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += SaveLog;
            aTimer.Interval = 1000; // Log every second
            aTimer.Start();
        }

        private void SaveLog(object source, ElapsedEventArgs e)
        {
            foreach (var ball in balls)
            {
                JsonSerializer.SerializeAsync(filestream, ball);
            }
            filestream.Flush();
        }
    }
}
