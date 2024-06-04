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

//Dawid Oszmiańczuk, Wiktor Żelechowski

//Główne okno aplikacji, które inicjalizuje ViewModel, BallService i odpowiada na interakcje użytkownika.

namespace BallSimulator
{
    public partial class MainWindow : Window
    {
        private ViewModel gameViewModel;
        private IDisposable gameUpdateSubscription;
        private Prezentacja ballRenderer;
        private Subject<Unit> startGameSubject;
        private Subject<Unit> updateGameSubject;

        public MainWindow()
        {
            InitializeComponent();
            gameViewModel = new ViewModel(new BallService(800, 450));
            DataContext = gameViewModel;
            ballRenderer = new Prezentacja();
            numBallsPicker.TextChanged += NumBallsPicker_TextChanged;

            startGameSubject = new Subject<Unit>();
            updateGameSubject = new Subject<Unit>();

            startGameSubject
                .SelectMany(_ => Observable.Interval(TimeSpan.FromMilliseconds(16)))
                .Subscribe(_ =>
                {
                    updateGameSubject.OnNext(Unit.Default);
                });

            updateGameSubject
                .Subscribe(_ =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        gameViewModel.UpdateGame();
                        ballRenderer.DrawBalls(canvas, gameViewModel.Balls);
                    });
                });

            this.Closed += (s, e) => DisposeSubscriptions();
        }

        private void DisposeSubscriptions()
        {
            gameUpdateSubscription?.Dispose();
            startGameSubject?.OnCompleted();
            updateGameSubject?.OnCompleted();
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
                startGameSubject.OnNext(Unit.Default);
            }
        }
    }
}
