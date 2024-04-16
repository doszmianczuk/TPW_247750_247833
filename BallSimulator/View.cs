﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BallSimulator
{
    public partial class MainWindow : Window
    {
        private ViewModel gameViewModel;
        private DispatcherTimer gameTimer;
        private Prezentacja ballRenderer;

        public MainWindow()
        {
            InitializeComponent();
            gameViewModel = new ViewModel(new BallService(800, 450)); //dajemy szerokosc i wysokosc canvas (tam gdzie kulki sie poruszaja)
            DataContext = gameViewModel;
            ballRenderer = new Prezentacja();
        }

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

        private void DrawBalls()
        {
            canvas.Children.Clear();
            foreach (var ball in gameViewModel.Balls)
            {
                var ellipse = new Ellipse
                {
                    Fill = Brushes.Blue,
                    Width = ball.Diameter,
                    Height = ball.Diameter
                };
                 
                Canvas.SetLeft(ellipse, ball.X - ball.Diameter / 2); //centrowanie kulki na jej wspolrz x
                Canvas.SetTop(ellipse, ball.Y - ball.Diameter / 2);  //centrowanie kulki na jej wspolrz y
                canvas.Children.Add(ellipse);
            }
        }
    }
}