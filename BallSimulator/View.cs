using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Reactive.Linq;
//Dawid Oszmiańczuk, Wiktor Żelechowski

//Główne okno aplikacji, które inicjalizuje ViewModel, BallService i odpowiada na interakcje użytkownika.

namespace BallSimulator
{
    public partial class MainWindow : Window
    {
        private ViewModel gameViewModel; // ViewModel dla gry.
        private DispatcherTimer gameTimer; // Timer do cyklicznego aktualizowania stanu gry.
        private Prezentacja ballRenderer; // Obiekt do rysowania piłek na kanwie.

        public MainWindow()
        {
            InitializeComponent(); // Metoda inicjalizująca komponenty UI zdefiniowane w XAML.
            gameViewModel = new ViewModel(new BallService(800, 450)); // Inicjalizacja ViewModel z serwisem piłek.
            DataContext = gameViewModel; // Ustawienie DataContext dla bindowania.
            ballRenderer = new Prezentacja(); // Inicjalizacja renderer'a piłek.
        }

        // Metody obsługi zdarzeń StartButton_Click, GameTimer_Tick

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            gameTimer?.Stop();
            if (int.TryParse(numBallsPicker.Text, out int numBalls))
            {
                gameViewModel.StartGame(numBalls, (int)canvas.ActualWidth, (int)canvas.ActualHeight);
            }

            gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) }; //~60fps
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            gameViewModel.UpdateGame();
            ballRenderer.DrawBalls(canvas, gameViewModel.Balls);
        }

    }
}
