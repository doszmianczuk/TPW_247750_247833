using Microsoft.VisualStudio.TestTools.UnitTesting;
using BallSimulator;
using System.Linq;
using Moq;

namespace Testy
{
    [TestClass]
    public class DataTests
    {
        private Mock<IDataService> _mockDataService;
        private Model _testBall;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockDataService = new Mock<IDataService>();
            _testBall = new Model { X = 10, Y = 10, VelocityX = 1, VelocityY = 1, Diameter = 5 };
        }

        [TestMethod]
        public void AddBall_ShouldAddNewBall()
        {
            // Arrange
            var balls = new List<Model>();
            _mockDataService.Setup(x => x.AddBall(It.IsAny<Model>()))
                .Callback<Model>(ball => balls.Add(ball));

            // Act
            _mockDataService.Object.AddBall(_testBall);

            // Assert
            _mockDataService.Verify(x => x.AddBall(_testBall), Times.Once);
            Assert.IsTrue(balls.Contains(_testBall));
        }

        [TestMethod]
        public void RemoveBall_ShouldRemoveExistingBall()
        {
            // Arrange
            var balls = new List<Model> { _testBall };
            _mockDataService.Setup(x => x.GetBalls()).Returns(balls);
            _mockDataService.Setup(x => x.RemoveBall(_testBall)).Returns(true)
                .Callback(() => balls.Remove(_testBall));

            // Act
            bool result = _mockDataService.Object.RemoveBall(_testBall);

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(balls.Contains(_testBall));
            _mockDataService.Verify(x => x.RemoveBall(_testBall), Times.Once);
        }

        [TestMethod]
        public void ClearBalls_ShouldRemoveAllBalls()
        {
            // Arrange
            var balls = new List<Model> { _testBall, _testBall };
            _mockDataService.Setup(x => x.GetBalls()).Returns(balls);
            _mockDataService.Setup(x => x.ClearBalls()).Callback(() => balls.Clear());

            // Act
            _mockDataService.Object.ClearBalls();

            // Assert
            Assert.IsFalse(balls.Any());
            _mockDataService.Verify(x => x.ClearBalls(), Times.Once);
        }
    }
}
