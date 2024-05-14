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
        void MoveBalls();
        IEnumerable<Model> GetBalls();
    }

    public class BallService : IBallService
    {
        private List<Model> balls = new List<Model>(); // Lista przechowująca piłki.
        private Random random = new Random(); // Generator losowych wartości dla początkowej prędkości i pozycji piłek.
        private int gameWidth, gameHeight; // Rozmiary obszaru gry.


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
            float dx = ball2.X - ball1.X;
            float dy = ball2.Y - ball1.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            // Uniknięcie dzielenia przez zero
            if (distance == 0) return;

            float nx = dx / distance; // x component of the normal vector
            float ny = dy / distance; // y component of the normal vector

            // Rozkład prędkości na składowe normalne i styczne
            float v1n = ball1.VelocityX * nx + ball1.VelocityY * ny; // normal component
            float v1t = -ball1.VelocityX * ny + ball1.VelocityY * nx; // tangential component

            float v2n = ball2.VelocityX * nx + ball2.VelocityY * ny;
            float v2t = -ball2.VelocityX * ny + ball2.VelocityY * nx;

            // Zastosowanie wzorów na zderzenie elastyczne
            float v1nAfter = (v1n * (ball1.Mass - ball2.Mass) + 2 * ball2.Mass * v2n) / (ball1.Mass + ball2.Mass);
            float v2nAfter = (v2n * (ball2.Mass - ball1.Mass) + 2 * ball1.Mass * v1n) / (ball1.Mass + ball2.Mass);

            // Złożenie składowych z powrotem do prędkości wektorowej
            ball1.VelocityX = v1nAfter * nx - v1t * ny;
            ball1.VelocityY = v1nAfter * ny + v1t * nx;
            ball2.VelocityX = v2nAfter * nx - v2t * ny;
            ball2.VelocityY = v2nAfter * ny + v2t * nx;
        }














        private void CheckCollisionWithWalls(Model ball)
        {
            if (ball.X - ball.Diameter / 2 <= 0 || ball.X + ball.Diameter / 2 >= gameWidth)
            {
                ball.VelocityX = -ball.VelocityX; // Odbicie od ściany w poziomie.
            }
            if (ball.Y - ball.Diameter / 2 <= 0 || ball.Y + ball.Diameter / 2 >= gameHeight)
            {
                ball.VelocityY = -ball.VelocityY; // Odbicie od ściany w pionie.
            }
        }

        public void InitializeBalls(int count, int gameWidth, int gameHeight)
        {
            int countMass1 = 0, countMass2 = 0;
            balls.Clear(); // Czyszczenie listy piłek.
            for (int i = 0; i < count; i++)
            {
                float mass = GenerateRandomMass();
                if (mass == 1) countMass1++;
                if (mass == 2) countMass2++;

                balls.Add(new Model
                {
                    X = random.Next(10, gameWidth - 10),
                    Y = random.Next(10, gameHeight - 10),
                    VelocityX = GenerateRandomVelocity(),
                    VelocityY = GenerateRandomVelocity(),
                    Diameter = 10,
                    Mass = mass
                });
            }
            Console.WriteLine($"Mass 1: {countMass1}, Mass 2: {countMass2}");
        }


        private float GenerateRandomVelocity()
        {
            // v od -2 do 2 .jesli chcemy wieksze V np od -3 do 3 to wtedy wpisujemy: ... * 9 - 3)
            return (float)(random.NextDouble() * 4 - 2);
        }
        private float GenerateRandomMass()
        {
            // Losuje liczbę 0 lub 1, a następnie dodaje 1, aby otrzymać 1 lub 2 jako wynik
            return random.Next(2) + 1;
        }


        private readonly object lockObject = new object();

        public void MoveBalls()
        {
            QuadTree quadTree = new QuadTree(0, new Rect(0, 0, gameWidth, gameHeight));
            foreach (var ball in balls)
            {
                quadTree.insert(ball);
            }

            foreach (var ball in balls)
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

                ball.X += ball.VelocityX;
                ball.Y += ball.VelocityY;
                CheckCollisionWithWalls(ball);
            }
        }



        public IEnumerable<Model> GetBalls()
        {
            return balls;
        }


    }


}