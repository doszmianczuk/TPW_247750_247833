using BallSimulator;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Zawiera definicję modelu piłki, w tym jej pozycję, prędkość i średnicę.
namespace BallSimulator
{
    public class Model
    {
        private readonly object _lock = new object();
        private float _x, _y, _velocityX, _velocityY, _diameter, _mass;
        private int _gameWidth, _gameHeight;

        public float X
        {
            get { return _x; }
            set { lock (_lock) { _x = value; } }
        }

        public float Y
        {
            get { return _y; }
            set { lock (_lock) { _y = value; } }
        }

        public float VelocityX
        {
            get { lock (_lock) { return _velocityX; } }
            set { lock (_lock) { _velocityX = value; } }
        }

        public float VelocityY
        {
            get { lock (_lock) { return _velocityY; } }
            set { lock (_lock) { _velocityY = value; } }
        }

        public float Diameter
        {
            get { return _diameter; }
            set { _diameter = value; }
        }

        public float Mass
        {
            get { return _mass; }
            set { _mass = value; }
        }

        public int GameWidth
        {
            get { return _gameWidth; }
            set { _gameWidth = value; }
        }

        public int GameHeight
        {
            get { return _gameHeight; }
            set { _gameHeight = value; }
        }
    }
}

