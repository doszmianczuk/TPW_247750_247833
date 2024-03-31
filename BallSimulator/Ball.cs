using BallSimulator;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallSimulator
{
    public class Ball
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }

        public float Diameter { get; set; }

        public void UpdatePosition()
        {
            X += VelocityX;
            Y += VelocityY;
        }


        public void CheckCollision(int gameWidth, int gameHeight)
        {
            if (X < 0 || X > gameWidth) VelocityX = -VelocityX;
            if (Y < 0 || Y > gameHeight) VelocityY = -VelocityY;
        }



    }


}