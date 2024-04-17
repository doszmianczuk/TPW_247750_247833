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
            balls.Clear(); // Czyszczenie listy piłek.
            for (int i = 0; i < count; i++)
            {
                balls.Add(new Model
                {
                    X = random.Next(10, gameWidth - 10), // Losowa pozycja X.
                    Y = random.Next(10, gameHeight - 10), // Losowa pozycja Y.
                    VelocityX = GenerateRandomVelocity(), // Losowa prędkość X.
                    VelocityY = GenerateRandomVelocity(), // Losowa prędkość Y.
                    Diameter = 10 // Ustalona średnica piłki.
                });
            }
        }

        private float GenerateRandomVelocity()
        {
            // v od -2 do 2 .jesli chcemy wieksze V np od -3 do 3 to wtedy wpisujemy: ... * 9 - 3)
            return (float)(random.NextDouble() * 4 - 2);
        }



        public void MoveBalls()
        {
            foreach (var ball in balls)
            {
                ball.X += ball.VelocityX; // Przesunięcie piłki w poziomie.
                ball.Y += ball.VelocityY; // Przesunięcie piłki w pionie.
                CheckCollisionWithWalls(ball); // Sprawdzenie kolizji z ścianami.
            }
        }

        public IEnumerable<Model> GetBalls()
        {
            return balls;
        }


    }


}