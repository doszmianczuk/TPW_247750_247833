using Microsoft.VisualStudio.TestTools.UnitTesting;
using BallSimulator;
using System.Linq;

namespace Testy
{
    [TestClass]
    public class DataTests
    {
        private Data _data;
        private Model _testBall;

        [TestInitialize]
        public void TestInitialize()
        {
            _data = new Data();
            _testBall = new Model { X = 10, Y = 10, VelocityX = 1, VelocityY = 1, Diameter = 5 };
        }

        [TestMethod]
        public void AddBall_ShouldAddNewBall()
        {
            // Act
            _data.AddBall(_testBall);

            // Assert
            CollectionAssert.Contains(_data.GetBalls().ToList(), _testBall);
        }

        [TestMethod]
        public void RemoveBall_ShouldRemoveSpecifiedBall()
        {
            // Arrange
            _data.AddBall(_testBall);

            // Act
            _data.RemoveBall(_testBall);

            // Assert
            CollectionAssert.DoesNotContain(_data.GetBalls().ToList(), _testBall);
        }

        [TestMethod]
        public void ClearBalls_ShouldRemoveAllBalls()
        {
            // Arrange
            _data.AddBall(_testBall);

            // Act
            _data.ClearBalls();

            // Assert
            Assert.IsFalse(_data.GetBalls().Any());
        }

        [TestMethod]
        public void GetVelocity_ShouldReturnVelocityOfExistingBall()
        {
            // Arrange
            _data.AddBall(_testBall);

            // Act
            var velocity = _data.GetVelocity(_testBall);

            // Assert
            Assert.AreEqual(1, velocity); // Assuming GetVelocity should return the VelocityX for this example
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void GetVelocity_ShouldThrowWhenBallNotFound()
        {
            // Arrange
            var nonExistingBall = new Model { X = 20, Y = 20, VelocityX = 2, VelocityY = 2, Diameter = 5 };

            // Act & Assert
            _data.GetVelocity(nonExistingBall);
        }
    }
}
