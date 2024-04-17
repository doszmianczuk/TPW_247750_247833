using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

//Rysuje piłki na kanwie, używając danych z modeli. Każda piłka jest reprezentowana jako Ellipse.

namespace BallSimulator
{
    public class Prezentacja
    {
        public void DrawBalls(Canvas canvas, IEnumerable<Model> balls)
        {
            canvas.Children.Clear(); // Czyści kanwę przed rysowaniem nowych obiektów.
            foreach (var ball in balls)
            {
                var ellipse = new Ellipse
                {
                    Fill = Brushes.Blue, // Kolor wypełnienia elipsy.
                    Width = ball.Diameter, // Szerokość elipsy zależna od średnicy piłki.
                    Height = ball.Diameter, // Wysokość elipsy równa szerokości.
                };

                Canvas.SetLeft(ellipse, ball.X - ball.Diameter / 2); // Ustawienie elipsy na kanwie.
                Canvas.SetTop(ellipse, ball.Y - ball.Diameter / 2); // Ustawienie elipsy na kanwie.
                canvas.Children.Add(ellipse); // Dodanie elipsy do kanwy.
            }
        }
    }


}
