using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipAI;
using System.ComponentModel.Composition;

namespace BattleshipApi
{
    
    /// <summary>
    /// This class implements a Medium level AI for the battleship game. It has two modes: 1. Hunt mode 2. Target mode.
    /// The hunt mode randomly chooses an odd bos (or square) from the battleship board. (Random hits with pairity)
    /// The target mode is activate when a hit is observed, and then the algorithm maintains a queue containing the nearby squares which 
    /// has more probability to have the subsequent blocks of the boat.
    /// The game starts with hunt mode.
    /// </summary>
    class BattleshipPlayAI_Inter : IBattleshipPlayAI
    {
        //Holds the queue of coordinates during the target mode.
        Stack<Coordinate> targetModeCoordinates;
        //Holds the present known state of the battleship board.
        int[,] battleshipBoard = new int[Constants.XMAX, Constants.YMAX];
        List<Coordinate> hitCoordinates = new List<Coordinate>();
        List<Coordinate> oddCoordinates;
        List<Coordinate> evenCoordinates;
        bool folTrend = false;


        /// <summary>
        /// This function returns a valid move for the battleship game.
        /// </summary>
        /// <param name="prevHit">Coordinates of the previous hit</param>
        /// <param name="hitResult">Result of the previous hit (1-Hit/2-Miss)</param>
        /// <param name="boatDestroyed">Returns the details of the destroyed boat, if any</param>
        /// <returns>Coordinate for the next hit</returns>
        public Coordinate GetMove(Coordinate prevHit, int hitResult, Boats boatDestroyed)
        {
            
            if (prevHit == null)       //First move
            {
                //Initializing function
                _init();
                //For the first move we directly jump to Hunt mode
                return _huntMode();
            }
            //Console.Write(prevHit.x + "\t" + prevHit.y + "\t");
            //Remove prevHit from odd and even coordinates
            if (prevHit.x % 2 == prevHit.y % 2)
            {
                for (int i = 0; i < evenCoordinates.Count(); i++)
                {
                    if (evenCoordinates.ElementAt(i).contains(prevHit))
                    {
                        evenCoordinates.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < oddCoordinates.Count(); i++)
                {
                    if (oddCoordinates.ElementAt(i).contains(prevHit))
                    {
                        oddCoordinates.RemoveAt(i);
                        break;
                    }
                }
            }

            battleshipBoard[prevHit.x, prevHit.y] = hitResult;
            if (hitResult == 2)         //Miss
            {
                //Console.Write("Miss\n");
                if (folTrend)
                {
                    folTrend = false;
                    return followTrend(hitCoordinates.ElementAt(0), prevHit);
                }
                if (targetModeCoordinates.Count == 0)       //If the targetmode queue is empty
                {
                    //Stack is empty, go back to hunt mode
                    hitCoordinates.Clear();
                    targetModeCoordinates.Clear();
                    folTrend = false;
                    return _huntMode();
                }
                else
                {
                    //go to target mode
                    folTrend = false;
                    return _targetMode();
                }
            }
            else if (hitResult == 1)    //Hit
            {
                //Console.Write("Hit\n");
                //Add code to destroy the queue when we get the information that a boat is destroyed
                //Adding the adjacent members of prevHit to stack

                if (boatDestroyed != Boats.none)
                {
                    targetModeCoordinates.Clear();
                    hitCoordinates.Clear();
                    folTrend = false;
                    return _huntMode();
                }
                hitCoordinates.Add(prevHit);
                if (hitCoordinates.Count == 1)
                {
                    List<Coordinate> adjCoordinates = getAdjacent(prevHit);
                    foreach (var point in adjCoordinates)
                    {
                        if (checkCoordinate(point))
                        {
                            targetModeCoordinates.Push(point);
                        }
                    }
                    //Go to target mode if any adjacent node is not hit
                    if (targetModeCoordinates.Count != 0)
                    {
                        folTrend = false;
                        return _targetMode();
                    }
                    else
                    {
                        folTrend = false;
                        return _huntMode();
                    }
                }
                targetModeCoordinates.Clear();
                folTrend = true;
                return followTrend(prevHit,hitCoordinates.ElementAt(0));
            }
            else
            {
                //Console.Write("Error:Hit result");
                return null;
            }
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
            if (coordinate.x - 1 >= 0)
            {
                Coordinate top = new Coordinate();
                top.x = coordinate.x - 1;
                top.y = coordinate.y;
                adjCoordinates.Add(top);
            }

            //Right
            if (coordinate.y + 1 < Constants.YMAX)
            {
                Coordinate right = new Coordinate();
                right.x = coordinate.x;
                right.y = coordinate.y + 1;
                adjCoordinates.Add(right);
            }

            //Bottom
            if (coordinate.x + 1 < Constants.XMAX)
            {
                Coordinate bottom = new Coordinate();
                bottom.x = coordinate.x + 1;
                bottom.y = coordinate.y;
                adjCoordinates.Add(bottom);
            }

            //Left
            if (coordinate.y - 1 >= 0)
            {
                Coordinate left = new Coordinate();
                left.x = coordinate.x;
                left.y = coordinate.y - 1;
                adjCoordinates.Add(left);
            }

            return adjCoordinates;
        }

        bool checkCoordinate(Coordinate c)
        {

            if (c != null && battleshipBoard[c.x, c.y] == 0)
            {
                return true;
            }
            return false;
        }

        private Coordinate _targetMode()
        {
            //Console.Write("Target Mode\t");
            return targetModeCoordinates.Pop();
        }

        /// <summary>
        /// Implements the Hunt mode for the game
        /// </summary>
        /// <returns>Coordinate for the next move</returns>
        private Coordinate _huntMode()
        {
            //Console.Write("Hunt Mode\t");
            //Randomly generate coordinates  ******Should modify to  use parity rule**********
            //var random = new Random();
            var random = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));
            Coordinate randCoordinate;
            int ran;

            if (oddCoordinates.Count() != 0)
            {
                ran = (int)random.Next(oddCoordinates.Count());
                randCoordinate = oddCoordinates.ElementAt(ran);
                if (battleshipBoard[randCoordinate.x, randCoordinate.y] != 0)
                {
                    Console.Write("Crap");
                }
                return randCoordinate;
            }
            ran = (int)random.Next(evenCoordinates.Count());
            randCoordinate = evenCoordinates.ElementAt(ran);
            return randCoordinate;
        }

        /// <summary>
        /// Generates a random odd number within the given limits.
        /// </summary>
        /// <param name="maxVal">The limit for the random number generated.</param>
        /// <returns>A random odd integer</returns>
        private int _randomOddNumber(int maxVal)
        {
            var random = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));
            maxVal = (maxVal % 2 == 0) ? maxVal : maxVal - 1;
            int temp = (random.Next(maxVal) * 2) % maxVal + 1;
            return temp;
        }


        /// <summary>
        /// /// Generates a random even number within the given limits.
        /// </summary>
        /// <param name="maxVal">The limit for the random number generated.</param>
        /// <returns>A random odd integer</returns>
        private int _randomEvenNumber(int maxVal)
        {
            var random = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));
            int temp = (random.Next(maxVal) * 2) % maxVal;
            return temp;
        }

        /// <summary>
        /// Initializes the game.
        /// </summary>
        private void _init()
        {
            //Console.Write("\nInitializing\n");
            targetModeCoordinates = new Stack<Coordinate>();
            battleshipBoard = new int[Constants.XMAX, Constants.YMAX];
            hitCoordinates = new List<Coordinate>();
            oddCoordinates = new List<Coordinate>();
            evenCoordinates = new List<Coordinate>();
            for (int i = 0; i < Constants.XMAX; i++)
            {
                for (int j = 0; j < Constants.YMAX; j++)
                {
                    if (i % 2 == j % 2)
                    {
                        evenCoordinates.Add(new Coordinate(i, j));
                    }
                    else
                    {
                        oddCoordinates.Add(new Coordinate(i, j));
                    }
                }
            }
        }

        

        
        /// <summary>
        /// Given the start and the end coordinate will follow the current trend and returns a vald move. (For Target mode only).
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private Coordinate followTrend(Coordinate start, Coordinate end)
        {
            //Console.Write("Follow Trend\t");
            char dir = findDirection(start, end);
            char oppdirection = 'e';
            switch (dir)
            {
                case 'u': oppdirection = 'd'; break;
                case 'd': oppdirection = 'u'; break;
                case 'l': oppdirection = 'r'; break;
                case 'r': oppdirection = 'l'; break;
            }
            Coordinate toHitNext = getValidMove(start, oppdirection);
            if (toHitNext == null)
            {
                toHitNext = getValidMove(start, dir);
                if (toHitNext == null)
                {
                    targetModeCoordinates.Clear();
                    hitCoordinates.Clear();
                    folTrend = false;
                    toHitNext = _huntMode();
                }
            }
            return toHitNext;
        }

        /// <summary>
        /// Given a starting coordinate and a direction, will give a valid move (For target mode only)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="direction"></param>
        /// <returns>Coordinate of the move</returns>
        private Coordinate getValidMove(Coordinate start, char direction)
        {
            int i = start.x, j = start.y;
            Coordinate validmove = null;
            bool flag = false;
            switch (direction)
            {
                case 'u':
                    while (i - 1 >= 0)
                    {
                        i--;
                        flag = false;
                        foreach (Coordinate item in hitCoordinates)
                        {
                            flag = item.contains(new Coordinate(i, j));
                            if (flag)
                            { break; }

                        }
                        if (!flag)
                        { break; }
                    }
                    break;
                case 'd':
                    while (i + 1 < Constants.XMAX)
                    {
                        i++;
                        flag = false;
                        foreach (Coordinate item in hitCoordinates)
                        {
                            flag = item.contains(new Coordinate(i, j));
                            if (flag)
                            { break; }

                        }
                        if (!flag)
                        { break; }
                    }

                    break;
                case 'l':
                    while (j - 1 >= 0)
                    {
                        j--;
                        flag = false;

                        foreach (Coordinate item in hitCoordinates)
                        {
                            flag = item.contains(new Coordinate(i, j));
                            if (flag)
                            { break; }

                        }
                        if (!flag)
                        { break; }
                    }
                    break;
                case 'r':
                    while (j + 1 < Constants.YMAX)
                    {
                        j++;
                        flag = false;
                        foreach (Coordinate item in hitCoordinates)
                        {
                            flag = item.contains(new Coordinate(i, j));
                            if (flag)
                            { break; }

                        }
                        if (!flag)
                        { break; }
                    }
                    break;
            }
            if (battleshipBoard[i, j] == 0)
            { validmove = new Coordinate(i, j); }
            return validmove;
        }

        /// <summary>
        /// Given to coordinates, finds the direction from start to end
        /// </summary>
        /// <param name="start">Starting Coordinate</param>
        /// <param name="end">Ending Coordinate</param>
        /// <returns>The direction ('u':up, 'd':down, 'r':right, 'l':left)</returns>
        private char findDirection(Coordinate start, Coordinate end)
        {
            char dir;
            if (start.x != end.x)
            {
                dir = (start.x < end.x) ? 'd' : 'u';
            }
            else if (start.y != end.y)
            {
                dir = (start.y < end.y) ? 'r' : 'l';
            }
            else { dir = 'e'; }
            return dir;
        }

        public String levelName()
        {
            return "Inter";
        }

    }
}
