using BallSimulator;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//ViewModel zarządza stanem aplikacji, używając BallService do manipulacji piłkami i aktualizacji ich stanu na interfejsie użytkownika.

namespace BallSimulator
{
    public class ViewModel
    {
        private readonly BallService ballService; // Serwis zarządzający logiką piłek.
        public ObservableCollection<Model> Balls { get; private set; } // Kolekcja piłek do bindowania z UI.

        public ViewModel(BallService ballService)
        {
            this.ballService = ballService; // Inicjalizacja serwisu piłek.
            this.Balls = new ObservableCollection<Model>(); // Inicjalizacja kolekcji.
        }

        public void StartGame(int ballCount, int gameWidth, int gameHeight)
        {
            ballService.InitializeBalls(ballCount, gameWidth, gameHeight); // Inicjalizacja piłek.
            UpdateBallsCollection(); // Aktualizacja kolekcji piłek.
        }

        public void UpdateGame()
        {
            ballService.MoveBalls(); // Ruch piłek.
            UpdateBallsCollection(); // Aktualizacja kolekcji.
        }

        private void UpdateBallsCollection()
        {
            Balls.Clear(); // Czyszczenie kolekcji.
            foreach (var ball in ballService.GetBalls())
            {
                Balls.Add(ball); // Dodawanie piłek do kolekcji.
            }
        }
    }


}