using Microsoft.VisualStudio.TestTools.UnitTesting;
using BallSimulator;
using System.Linq;
using Moq;

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
        public void ClearBalls_ShouldRemoveAllBalls()
        {
            // Arrange
            _data.AddBall(_testBall);

            // Act
            _data.ClearBalls();

            // Assert
            Assert.IsFalse(_data.GetBalls().Any());
        }
    }
}
