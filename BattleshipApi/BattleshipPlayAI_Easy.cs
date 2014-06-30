using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipAI;

namespace BattleshipApi
{
    class BattleshipPlayAI_Easy : IBattleshipPlayAI
    {
        static int[,] battleshipBoard = new int[Constants.XMAX, Constants.YMAX];
        public Coordinate GetMove(Coordinate prevHit, int hitResult, Boats boatDestroyed)
        {
           
            if (prevHit == null)
            {
                _init();
                return HuntMode();
            }
            battleshipBoard[prevHit.x, prevHit.y] = hitResult;
            if (hitResult == 2)
            {
                return HuntMode();
            }

            return TargetMode(prevHit);
            
        }

        private void _init()
        {
            battleshipBoard = new int[Constants.XMAX, Constants.YMAX];
        }

        private Coordinate HuntMode()
        {
            Coordinate cord = new Coordinate();
            var random = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));

            do
            {
                cord.x = random.Next(Constants.XMAX);
                cord.y = random.Next(Constants.YMAX);
            } while (battleshipBoard[cord.x, cord.y] != 0);

            return cord;
        }

        private Coordinate TargetMode(Coordinate prevHit)
        {
            var random = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));

            List<Coordinate> adjacentCoordinates = getAdjacent(prevHit);

            if (adjacentCoordinates.Count == 0 || adjacentCoordinates == null)
            {
                return HuntMode();
            }

            return adjacentCoordinates.ElementAt(random.Next(adjacentCoordinates.Count()));
        }

        bool checkCoordinate(Coordinate c)
        {

            if (c != null && battleshipBoard[c.x, c.y] == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Generates the list of adjacent vertices for the board
        /// </summary>
        /// <param name="coordinate">The coordinate for which you want to generate the adjacent coordinates.</param>
        /// <returns>The list containing adjacent coordinates</returns>
        public List<Coordinate> getAdjacent(Coordinate coordinate)
        {
            List<Coordinate> adjCoordinates = new List<Coordinate>();
            
            //Top
            if (coordinate.x - 1 >= 0 && checkCoordinate(new Coordinate(coordinate.x - 1, coordinate.y)))
            {
                Coordinate top = new Coordinate();
                top.x = coordinate.x - 1;
                top.y = coordinate.y;
                adjCoordinates.Add(top);
            }

            //Right
            if (coordinate.y + 1 < Constants.YMAX && checkCoordinate(new Coordinate(coordinate.x, coordinate.y + 1)))
            {
                Coordinate right = new Coordinate();
                right.x = coordinate.x;
                right.y = coordinate.y + 1;
                adjCoordinates.Add(right);
            }

            //Bottom
            if (coordinate.x + 1 < Constants.XMAX && checkCoordinate(new Coordinate(coordinate.x + 1, coordinate.y)))
            {
                Coordinate bottom = new Coordinate();
                bottom.x = coordinate.x + 1;
                bottom.y = coordinate.y;
                adjCoordinates.Add(bottom);
            }

            //Left
            if (coordinate.y - 1 >= 0 && checkCoordinate(new Coordinate(coordinate.x, coordinate.y - 1)))
            {
                Coordinate left = new Coordinate();
                left.x = coordinate.x;
                left.y = coordinate.y - 1;
                adjCoordinates.Add(left);
            }

            return adjCoordinates;
        }

        public String levelName()
        {
            return "Easy";
        }
    }
}
