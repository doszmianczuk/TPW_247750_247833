using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows; // dla Rectangle

namespace BallSimulator
{


    public class QuadTree
    {
        private int maxObjects = 10;
        private int maxLevels = 5;
        private int level;
        private List<Model> objects;
        private Rect bounds;
        private QuadTree[] nodes;

        public QuadTree(int pLevel, Rect pBounds)
        {
            level = pLevel;
            objects = new List<Model>();
            bounds = pBounds;
            nodes = new QuadTree[4];
        }

        // Podział węzła na 4 podwęzły
        private void split()
        {
            var subWidth = bounds.Width / 2;
            var subHeight = bounds.Height / 2;
            var x = bounds.X;
            var y = bounds.Y;

            nodes[0] = new QuadTree(level + 1, new Rect(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new QuadTree(level + 1, new Rect(x, y, subWidth, subHeight));
            nodes[2] = new QuadTree(level + 1, new Rect(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new QuadTree(level + 1, new Rect(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        // Określenie, w którym węźle znajduje się dany obiekt
        private int getIndex(Model model)
        {
            int index = -1;
            double verticalMidpoint = bounds.X + (bounds.Width / 2);
            double horizontalMidpoint = bounds.Y + (bounds.Height / 2);

            bool topQuadrant = (model.Y < horizontalMidpoint && model.Y + model.Diameter < horizontalMidpoint);
            bool bottomQuadrant = (model.Y > horizontalMidpoint);

            if (model.X < verticalMidpoint && model.X + model.Diameter < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 1;
                }
                else if (bottomQuadrant)
                {
                    index = 2;
                }
            }
            else if (model.X > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 0;
                }
                else if (bottomQuadrant)
                {
                    index = 3;
                }
            }

            return index;
        }

        // Wstawianie obiektu do QuadTree
        public void insert(Model model)
        {
            if (nodes[0] != null)
            {
                int index = getIndex(model);

                if (index != -1)
                {
                    nodes[index].insert(model);
                    return;
                }
            }

            objects.Add(model);

            if (objects.Count > maxObjects && level < maxLevels)
            {
                if (nodes[0] == null)
                {
                    split();
                }

                int i = 0;
                while (i < objects.Count)
                {
                    int index = getIndex(objects[i]);
                    if (index != -1)
                    {
                        nodes[index].insert(objects[i]);
                        objects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

     
        public List<Model> retrieve(List<Model> returnObjects, Model model)
        {
            int index = getIndex(model);
            if (index != -1 && nodes[0] != null)
            {
                nodes[index].retrieve(returnObjects, model);
            }

            returnObjects.AddRange(objects);

            return returnObjects;
        }

       
        public void clear()
        {
            objects.Clear();

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].clear();
                    nodes[i] = null;
                }
            }
        }
    }

}
