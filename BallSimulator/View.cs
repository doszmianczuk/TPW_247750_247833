//Dawid Oszmiańczuk, Wiktor Żelechowski

//Główne okno aplikacji, które inicjalizuje ViewModel, BallService i odpowiada na interakcje użytkownika.
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Reactive;
using System;


namespace BallSimulator
{
    public partial class MainWindow : Window
    {
        private ViewModel gameViewModel;
        private Prezentacja ballRenderer;
        private DispatcherTimer gameTimer;

        public MainWindow()
        {
            InitializeComponent();
            gameViewModel = new ViewModel(new BallService(800, 450), new Data());
            DataContext = gameViewModel;
            ballRenderer = new Prezentacja();
            numBallsPicker.TextChanged += NumBallsPicker_TextChanged;

            InitializeGameTimer();
        }

        private void InitializeGameTimer()
        {
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(16); // 60 FPS
            gameTimer.Tick += GameTimer_Tick;
        }

        private async void GameTimer_Tick(object sender, EventArgs e)
        {
            await gameViewModel.UpdateGameAsync();
            ballRenderer.DrawBalls(canvas, gameViewModel.Balls);
        }

        private void NumBallsPicker_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(numBallsPicker.Text, out int numBalls))
            {
                gameViewModel.StartGame(numBalls, (int)canvas.ActualWidth, (int)canvas.ActualHeight);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(numBallsPicker.Text, out int numBalls))
            {
                gameViewModel.StartGame(numBalls, (int)canvas.ActualWidth, (int)canvas.ActualHeight);
                gameTimer.Start();
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Stop();
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            gameViewModel.LogGameState("log.json");
        }
    }
}

