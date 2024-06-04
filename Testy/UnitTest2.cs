using Microsoft.VisualStudio.TestTools.UnitTesting;
using BallSimulator;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace BallSimulator.Tests
{
    [TestClass]
    public class BallServiceTests
    {
        private BallService _ballService;

        [TestInitialize]
        public void Initialize()
        {
            _ballService = new BallService(800, 600); // Inicjalizuje BallService z określoną szerokością i wysokością obszaru gry.
        }

        [TestMethod]
        public void InitializeBalls_CreatesCorrectNumberOfBalls()
        {
            int ballCount = 5; // Definiuje oczekiwaną liczbę piłek.
            _ballService.InitializeBalls(ballCount, 800, 600); // Inicjalizuje piłki w obszarze gry.

            Assert.AreEqual(ballCount, _ballService.GetBalls().Count()); // Sprawdza, czy liczba utworzonych piłek jest zgodna z oczekiwaną.
        }


        [TestMethod]
        public void MoveBalls_ChangesBallPositions()
        {
            _ballService.InitializeBalls(1, 800, 600); // Inicjalizuje jedną piłkę.
            var initialPosition = _ballService.GetBalls().First(); // Pobiera początkową pozycję piłki.
            float initialX = initialPosition.X;
            float initialY = initialPosition.Y;

            _ballService.MoveBalls(); // Wykonuje ruch piłek.
            var newPosition = _ballService.GetBalls().First(); // Pobiera nową pozycję piłki.

            Assert.AreNotEqual(initialX, newPosition.X); // Sprawdza, czy pozycja X piłki zmieniła się.
            Assert.AreNotEqual(initialY, newPosition.Y); // Sprawdza, czy pozycja Y piłki zmieniła się.
        }
        [TestClass]
        public class BallRendererTests
        {
            private Canvas _canvas;
            private Prezentacja _renderer;

            [TestInitialize]
            public void Initialize()
            {
                _canvas = new Canvas();
                _renderer = new Prezentacja();
            }
        }
    }
}