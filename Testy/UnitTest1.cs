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
            _testBall = new Model { X = 10, Y = 10, VelocityX = 1, VelocityY = 1, Diameter = 5 }; // Tworzenie testowej pi³ki.
        }


        [TestMethod]
        public void AddBall_ShouldAddNewBall()
        {
            // Arrange
            var balls = new List<Model>(); // Lista przechowuj¹ca pi³ki.
            _mockDataService.Setup(x => x.AddBall(It.IsAny<Model>()))
                .Callback<Model>(ball => balls.Add(ball)); // Konfiguracja mocka, aby dodawaæ pi³ki do listy.

            // Act
            _mockDataService.Object.AddBall(_testBall); // Wywo³anie metody dodaj¹cej pi³kê.

            // Assert
            _mockDataService.Verify(x => x.AddBall(_testBall), Times.Once); // Sprawdzenie, czy metoda zosta³a wywo³ana raz.
            Assert.IsTrue(balls.Contains(_testBall)); // Sprawdzenie, czy pi³ka zosta³a dodana.
        }

        [TestMethod]
        public void RemoveBall_ShouldRemoveExistingBall()
        {
            // Arrange
            var balls = new List<Model> { _testBall }; // Lista z jedn¹ pi³k¹.
            _mockDataService.Setup(x => x.GetBalls()).Returns(balls); // Zwraca listê pi³ek.
            _mockDataService.Setup(x => x.RemoveBall(_testBall)).Returns(true)
                .Callback(() => balls.Remove(_testBall)); // Konfiguracja mocka, aby usuwaæ pi³ki z listy.

            // Act
            bool result = _mockDataService.Object.RemoveBall(_testBall); // Wywo³anie metody usuwaj¹cej pi³kê.

            // Assert
            Assert.IsTrue(result); // Sprawdzenie, czy pi³ka zosta³a usuniêta.
            Assert.IsFalse(balls.Contains(_testBall)); // Sprawdzenie, czy pi³ka nie jest ju¿ na liœcie.
            _mockDataService.Verify(x => x.RemoveBall(_testBall), Times.Once); // Sprawdzenie, czy metoda zosta³a wywo³ana raz.
        }

        [TestMethod]
        public void ClearBalls_ShouldRemoveAllBalls()
        {
            // Arrange
            var balls = new List<Model> { _testBall, _testBall }; // Lista z dwiema pi³kami.
            _mockDataService.Setup(x => x.GetBalls()).Returns(balls); // Zwraca listê pi³ek.
            _mockDataService.Setup(x => x.ClearBalls()).Callback(() => balls.Clear()); // Konfiguracja mocka, aby czyœciæ listê.

            // Act
            _mockDataService.Object.ClearBalls(); // Wywo³anie metody czyœc¹cej listê.

            // Assert
            Assert.IsFalse(balls.Any()); // Sprawdzenie, czy lista jest pusta.
            _mockDataService.Verify(x => x.ClearBalls(), Times.Once); // Sprawdzenie, czy metoda zosta³a wywo³ana raz.
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
