using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BallSimulator
{
    public class Prezentacja
    {
        public void DrawBalls(Canvas canvas, IEnumerable<Model> balls)
        {
            canvas.Children.Clear();
            foreach (var ball in balls)
            {
                var ellipse = new Ellipse
                {
                    Fill = Brushes.Blue,
                    Width = ball.Diameter,
                    Height = ball.Diameter,
                };

                Canvas.SetLeft(ellipse, ball.X - ball.Diameter / 2);
                Canvas.SetTop(ellipse, ball.Y - ball.Diameter / 2);
                canvas.Children.Add(ellipse);
            }
        }
    }

}
