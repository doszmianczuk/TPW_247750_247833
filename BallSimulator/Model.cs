using BallSimulator;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallSimulator
{
    public class Model
    {
        private float _x;
        private float _y;
        private float _velocityX;
        private float _velocityY;
        private float _diameter;

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public float VelocityX
        {
            get { return _velocityX; }
            set { _velocityX = value; }
        }

        public float VelocityY
        {
            get { return _velocityY; }
            set { _velocityY = value; }
        }

        public float Diameter
        {
            get { return _diameter; }
            set { _diameter = value; }
        }
    }

}