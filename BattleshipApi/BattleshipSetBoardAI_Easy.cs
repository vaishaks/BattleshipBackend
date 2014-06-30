using BattleshipAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace BattleshipApi
{
    
    class BattleshipSetBoardAI_Easy : IBattleshipSetBoardAI
    {
        List<Coordinate> placement = new List<Coordinate>();
        static int[,] board = new int[Constants.YMAX, Constants.XMAX];
        /// <summary>
        /// Generates the initial coordinate of boats on the board.
        /// </summary>
        /// <returns>Coordinates of the start and end position of all the different boats</returns>
        public List<Coordinate> SetBoard()
        {

            int[] ships = new int[Constants.noOfBoats];

            for (int z = 0; z < Constants.noOfBoats; z++)
            {
                ships[z] = Constants.boatLength[z];
            }

                for (int i = 0; i < Constants.YMAX; i++)
                {
                    for (int j = 0; j < Constants.XMAX; j++)
                    {
                        board[i, j] = 0;
                    }
                }
            bool shipPlaced = false, vertical = false;
            int l = Constants.noOfBoats;
            for (int i = 0; i < l; i++)
            {
                //Console.Write("\nshipsSize : " + ships[i]);
                shipPlaced = false;
                //vertical = randomBoolean();
                var r = new Random();
                int value = r.Next(2);
                //Console.Write("\nvalue for random vertical : " + value);
                if (value == 0)
                {
                    vertical = false;
                }
                else
                {
                    vertical = true;
                }
                Coordinate pos = new Coordinate();
                while (!shipPlaced)
                {
                    r = new Random();
                    value = r.Next(Constants.XMAX);
                    //Console.Write("\nrandom x value is  : " + value);
                    pos.x = value;
                    //r = new Random();
                    value = r.Next(Constants.YMAX);
                    //Console.Write("\nrandom y value is  : " + value);
                    pos.y = value;
                    shipPlaced = placeShip(pos, ships[i], vertical, board);
                }
                
            }

            return placement;
        }
        bool placeShip(Coordinate pos, int shipSize, bool vertical, int[,] board)
        {
            int x = pos.x;
            int y = pos.y;
            //Console.Write("\nrandom x value inside placeShip is  : " + x);
            //Console.Write("\nrandom y value inside placeShip is  : " + y);
            int z;
            if (vertical)
            {
                z = y;
            }
            else
            {
                z = x;
            }
            int end = shipSize + z - 1;
            int SHIP = 1;
            if (shipCanOccupyPosition(SHIP, pos, shipSize, vertical, board))
            {
                //Console.Write("shipCanOccupyPosition");
                Coordinate c = new Coordinate();

                var i = z;
                if (vertical)
                {
                    c.x = i;
                    c.y = x;
                    placement.Add(c);
                    //board[x, i] = 1;
                }
                else
                {
                    c.x = y;
                    c.y = i;
                    placement.Add(c);
                    //board[i, y] = 1;
                }
                for (i = z; i <= end; i++)
                {
                    if (vertical)
                    {
                        board[i, x] = 1;
                        if (i == end)
                        {
                            Coordinate c1 = new Coordinate();
                            c1.x = i;
                            c1.y = x;
                            placement.Add(c1);
                        }
                    }
                    else
                    {
                        board[y, i] = 1;
                        if (i == end)
                        {
                            Coordinate c1 = new Coordinate();
                            c1.x = y;
                            c1.y = i;
                            placement.Add(c1);
                        }
                    }
                }

                return true;
            }
            else
            {
                //Console.Write("shipCanOccupyPosition is false");
            }
            return false;
        }
        bool shipCanOccupyPosition(int criteriaForRejection, Coordinate pos, int shipSize, bool vertical, int[,] board)
        {
            int x = pos.x;
            int y = pos.y;
            //Console.Write("\nrandom x value inside shipCanOccupyPosition is  : " + x);
            //Console.Write("\nrandom x value inside shipCanOccupyPosition is  : " + y);
            int z;

            int end;
            if (vertical)
            {
                z = y;
                //Console.Write("\n z inside shipCanOccupyPosition is  : " + z);
                end = z + shipSize - 1;
                //Console.Write("\n end inside shipCanOccupyPosition is  : " + end);
                if (end > Constants.YMAX - 1)
                {
                    //Console.Write("\n end > Constants.YMAX-1 : ");
                    return false;
                }
            }
            else
            {
                z = x;
               //Console.Write("\n z inside shipCanOccupyPosition is  : " + z);
                end = z + shipSize - 1;
                //Console.Write("\n end inside shipCanOccupyPosition is  : " + end);
                if (end > Constants.XMAX - 1)
                {
                    //Console.Write("\n end > Constants.YMAX-1 : ");
                    return false;
                }

            }
            for (int i = z; i <= end; i++)
            {
                int thisPos;
                if (vertical)
                {
                    thisPos = board[i, x];
                }
                else
                {
                    thisPos = board[y, i];
                }
                if (thisPos == criteriaForRejection)
                {
                    return false;
                }
            }
            return true;
        }


        public String levelName()
        {
            return "Easy";
        }
    }
}
