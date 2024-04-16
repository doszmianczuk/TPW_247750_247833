using NUnit.Framework;
using BallSimulator;
using Moq;
// TESTY NIE DZIALAJA!!!!!
namespace Tests
{
    [TestFixture]
    public class GameViewModelTests
    {
        private Mock<IBallService> mockBallService;
        private GameViewModel viewModel;

        [SetUp]
        public void Setup()
        {
            mockBallService = new Mock<IBallService>();
            viewModel = new GameViewModel(mockBallService.Object);
            mockBallService.Setup(service => service.GetBalls()).Returns(new List<Ball>());
        }

        [Test]
        public void StartGame_ShouldInvokeInitializeBallsOnBallService()
        {
           
            int ballCount = 10;
            int gameWidth = 800;
            int gameHeight = 450;

            viewModel.StartGame(ballCount, gameWidth, gameHeight);

            mockBallService.Verify(service => service.InitializeBalls(ballCount, gameWidth, gameHeight), Times.Once);
        }
    }
}
