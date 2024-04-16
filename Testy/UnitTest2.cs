using Microsoft.VisualStudio.TestTools.UnitTesting;
using BallSimulator;
using System.Linq;

namespace BallSimulator.Tests
{
    [TestClass]
    public class BallServiceTests
    {
        private BallService _ballService;

        [TestInitialize]
        public void Initialize()
        {
            _ballService = new BallService(800, 600);
        }

        [TestMethod]
        public void InitializeBalls_CreatesCorrectNumberOfBalls()
        {
            int ballCount = 5;
            _ballService.InitializeBalls(ballCount, 800, 600);

            Assert.AreEqual(ballCount, _ballService.GetBalls().Count());
        }

        [TestMethod]
        public void MoveBalls_ChangesBallPositions()
        {
            _ballService.InitializeBalls(1, 800, 600);
            var initialPosition = _ballService.GetBalls().First();
            float initialX = initialPosition.X;
            float initialY = initialPosition.Y;

            _ballService.MoveBalls();
            var newPosition = _ballService.GetBalls().First();

            Assert.AreNotEqual(initialX, newPosition.X);
            Assert.AreNotEqual(initialY, newPosition.Y);
        }
    }
}