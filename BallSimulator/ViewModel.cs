using BallSimulator;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallSimulator
{
    public class ViewModel
    {
        private readonly BallService ballService;
        public ObservableCollection<Model> Balls { get; private set; }

        public ViewModel(BallService ballService)
        {
            this.ballService = ballService;
            this.Balls = new ObservableCollection<Model>();
        }

        public void StartGame(int ballCount, int gameWidth, int gameHeight)
        {
            ballService.InitializeBalls(ballCount, gameWidth, gameHeight);
            UpdateBallsCollection();
        }

        public void UpdateGame()
        {
            ballService.MoveBalls();
            UpdateBallsCollection();
        }

        private void UpdateBallsCollection()
        {
            Balls.Clear();
            foreach (var ball in ballService.GetBalls())
            {
                Balls.Add(ball);
            }
        }
    }

}