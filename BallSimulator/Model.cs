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
        private float _x, _y, _velocityX, _velocityY, _diameter; // Prywatne zmienne dla modelu piłki.

        public float X // Właściwość publiczna dla pozycji X.
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y // Właściwość publiczna dla pozycji Y.
        {
            get { return _y; }
            set { _y = value; }
        }

        public float VelocityX // Właściwość publiczna dla prędkości w osi X.
        {
            get { return _velocityX; }
            set { _velocityX = value; }
        }

        public float VelocityY // Właściwość publiczna dla prędkości w osi Y.
        {
            get { return _velocityY; }
            set { _velocityY = value; }
        }

        public float Diameter // Właściwość publiczna dla średnicy piłki.
        {
            get { return _diameter; }
            set { _diameter = value; }
        }
    }


}