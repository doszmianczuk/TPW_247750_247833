using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Definiuje interfejs IDataService i jego implementację, która pozwala na zarządzanie kolekcją obiektów Model (piłek).

namespace BallSimulator
{
    public interface IDataService
    {
        IEnumerable<Model> GetBalls(); // Pobiera wszystkie piłki.
        void AddBall(Model ball); // Dodaje nową piłkę do kolekcji.
        void ClearBalls(); // Usuwa wszystkie piłki z kolekcji.
        bool RemoveBall(Model ball); // Usuwa określoną piłkę z kolekcji.
    }

    public class Data : IDataService
    {
        private List<Model> balls = new List<Model>(); // Prywatna lista przechowująca piłki.

        public IEnumerable<Model> GetBalls() => balls; // Zwraca listę piłek.

        public void AddBall(Model ball)
        {
            balls.Add(ball); // Dodaje piłkę do listy.
        }

        public void ClearBalls()
        {
            balls.Clear(); // Czyści listę piłek.
        }

        public bool RemoveBall(Model ball)
        {
            return balls.Remove(ball); // Usuwa piłkę z listy i zwraca, czy operacja się powiodła.
        }
    }


}