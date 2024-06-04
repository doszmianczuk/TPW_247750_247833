using Microsoft.VisualStudio.TestTools.UnitTesting;
using BallSimulator;
using System.Linq;
using Moq;

namespace Testy
{
    [TestClass]
    public class DataTests
    {
        private Mock<IDataService> _mockDataService; // Mock interfejsu IDataService.
        private Model _testBall; // Testowy obiekt Model.

        [TestInitialize]
        public void TestInitialize()
        {
            _mockDataService = new Mock<IDataService>(); // Inicjalizacja mocka.
            _testBall = new Model { X = 10, Y = 10, VelocityX = 1, VelocityY = 1, Diameter = 5 }; // Tworzenie testowej pi�ki.
        }


        [TestMethod]
        public void AddBall_ShouldAddNewBall()
        {
            // Arrange
            var balls = new List<Model>(); // Lista przechowuj�ca pi�ki.
            _mockDataService.Setup(x => x.AddBall(It.IsAny<Model>()))
                .Callback<Model>(ball => balls.Add(ball)); // Konfiguracja mocka, aby dodawa� pi�ki do listy.

            // Act
            _mockDataService.Object.AddBall(_testBall); // Wywo�anie metody dodaj�cej pi�k�.

            // Assert
            _mockDataService.Verify(x => x.AddBall(_testBall), Times.Once); // Sprawdzenie, czy metoda zosta�a wywo�ana raz.
            Assert.IsTrue(balls.Contains(_testBall)); // Sprawdzenie, czy pi�ka zosta�a dodana.
        }

        [TestMethod]
        public void RemoveBall_ShouldRemoveExistingBall()
        {
            // Arrange
            var balls = new List<Model> { _testBall }; // Lista z jedn� pi�k�.
            _mockDataService.Setup(x => x.GetBalls()).Returns(balls); // Zwraca list� pi�ek.
            _mockDataService.Setup(x => x.RemoveBall(_testBall)).Returns(true)
                .Callback(() => balls.Remove(_testBall)); // Konfiguracja mocka, aby usuwa� pi�ki z listy.

            // Act
            bool result = _mockDataService.Object.RemoveBall(_testBall); // Wywo�anie metody usuwaj�cej pi�k�.

            // Assert
            Assert.IsTrue(result); // Sprawdzenie, czy pi�ka zosta�a usuni�ta.
            Assert.IsFalse(balls.Contains(_testBall)); // Sprawdzenie, czy pi�ka nie jest ju� na li�cie.
            _mockDataService.Verify(x => x.RemoveBall(_testBall), Times.Once); // Sprawdzenie, czy metoda zosta�a wywo�ana raz.
        }

        [TestMethod]
        public void ClearBalls_ShouldRemoveAllBalls()
        {
            // Arrange
            var balls = new List<Model> { _testBall, _testBall }; // Lista z dwiema pi�kami.
            _mockDataService.Setup(x => x.GetBalls()).Returns(balls); // Zwraca list� pi�ek.
            _mockDataService.Setup(x => x.ClearBalls()).Callback(() => balls.Clear()); // Konfiguracja mocka, aby czy�ci� list�.

            // Act
            _mockDataService.Object.ClearBalls(); // Wywo�anie metody czy�c�cej list�.

            // Assert
            Assert.IsFalse(balls.Any()); // Sprawdzenie, czy lista jest pusta.
            _mockDataService.Verify(x => x.ClearBalls(), Times.Once); // Sprawdzenie, czy metoda zosta�a wywo�ana raz.
        }
        [TestMethod]
        public void LoggingDoesNotAffectBallBehavior()
        {
            // Arrange
            var data = new Data();
            var ball = new Model { Diameter = 5, Mass = 1 };
            data.AddBall(ball);
            var initialCount = data.GetBalls().Count();

            // Act
            data.LogDiagnostics("diagnostics.json");
            var countAfterLogging = data.GetBalls().Count();

            // Assert
            Assert.AreEqual(initialCount, countAfterLogging);
        }


    }


}
