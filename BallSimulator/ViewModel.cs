using BallSimulator;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BallSimulator
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly BallService ballService;
        public ObservableCollection<Model> Balls { get; private set; }

        public ViewModel(BallService ballService)
        {
            this.ballService = ballService;
            this.Balls = new ObservableCollection<Model>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void StartGame(int ballCount, int gameWidth, int gameHeight)
        {
            ballService.InitializeBalls(ballCount, gameWidth, gameHeight);
            UpdateBallsCollection();
        }

        public async Task UpdateGameAsync()
        {
            await ballService.MoveBallsAsync();
            UpdateBallsCollection();
        }

        private void UpdateBallsCollection()
        {
            Balls.Clear();
            foreach (var ball in ballService.GetBalls())
            {
                Balls.Add(ball);
            }
            OnPropertyChanged(nameof(Balls));
        }
    }
}
