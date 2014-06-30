using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipAI;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace BattleshipApi
{
    public class PlayGame
    {
        [ImportMany(typeof(IBattleshipPlayAI))]
        List<IBattleshipPlayAI> _playAlgorithms;

        public List<Coordinate> playGame(List<Coordinate> playerBoats)
        {
            //Initialization
            Compose();
            _init();

            int[,] board = new int[Constants.XMAX, Constants.YMAX];
            ConvertListToBoard(playerBoats, board);
            int hitCount = 0;
            int noOfturns = 0;
            int hitResult = 0;
            Coordinate prevHit = null;
            Boats boats = Boats.none;
            int[] noOfHits = new int[Constants.noOfBoats];//{ 2, 3, 3, 4, 5 };
            int totalHits = 0;

            for (int i = 0; i < Constants.noOfBoats; i++)
            {
                noOfHits[i] = Constants.boatLength[i];
                totalHits += noOfHits[i];
            }

            List<Coordinate> compCoordinates = new List<Coordinate>();

            int selectedIndex = 2 ;
            //Console.Write("Algorithms found:\n");
            //for (int i = 0; i < _playAlgorithms.Count(); i++)
            //{
            //    Console.Write(i + ": " + _playAlgorithms[i].levelName() + "\n");
            //}
            //Console.Write("Choose: ");
            //selectedIndex = Convert.ToInt16(Console.ReadLine());

            //List<Coordinate> coordinates = new BattleshipSetBoardAI_Easy().SetBoard();

            while (hitCount < totalHits)
            {
                prevHit = _playAlgorithms[selectedIndex].GetMove(prevHit, hitResult, boats);
                //Console.Write(prevHit.x + "\t" + prevHit.y +"\t"+hitCount+"\t"+totalHits+"\n");
                noOfturns++;
                compCoordinates.Add(prevHit);

                boats = Boats.none;

                if (board[prevHit.x, prevHit.y] > 0)
                {
                    hitCount++;
                    noOfHits[board[prevHit.x, prevHit.y] - 1]--;
                    if (noOfHits[board[prevHit.x, prevHit.y] - 1] == 0)
                    {
                        boats = (Boats)(board[prevHit.x, prevHit.y] - 1);
                    }
                   
                    hitResult = 1;
                    board[prevHit.x, prevHit.y] = -1;
                }
                else if (board[prevHit.x, prevHit.y] == 0)
                {
                    hitResult = 2;
                    board[prevHit.x, prevHit.y] = -1;
                }
                else
                {
                    compCoordinates.RemoveAt(compCoordinates.Count()-1);
                    //Console.Write("Error "+prevHit.x+"\t"+prevHit.y);
                    //Console.Read();
                }
            }
            return compCoordinates;
        }
        private void Compose()
        {
            AssemblyCatalog catalog = new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly());
            CompositionContainer container = new CompositionContainer(catalog);
            container.SatisfyImportsOnce(this);
        }

        private void _init()
        {

        }

        private void ConvertListToBoard(List<Coordinate> coordinates, int[,] board)
        {
            for (int i = 0; i < coordinates.Count(); i += 2)
            {
                Coordinate c1 = coordinates.ElementAt(i);
                Coordinate c2 = coordinates.ElementAt(i + 1);
                char dir = findDirection(c1, c2);
                int len = findLength(c1, c2);

                switch (dir)
                {
                    case 'u':
                        for (int j = 0; j < len; j++)
                        {
                            board[c1.x - j, c1.y] = (i / 2) + 1;
                        }
                        break;
                    case 'd':
                        for (int j = 0; j < len; j++)
                        {
                            board[c1.x + j, c1.y] = (i / 2) + 1;
                        }
                        break;
                    case 'l':
                        for (int j = 0; j < len; j++)
                        {
                            board[c1.x, c1.y - j] = (i / 2) + 1;
                        }
                        break;
                    case 'r':
                        for (int j = 0; j < len; j++)
                        {
                            board[c1.x, c1.y + j] = (i / 2) + 1;
                        }
                        break;
                    case 'e':
                        board[c1.x, c1.y] = (i / 2) + 1;
                        break;
                }

            }

            //for (int k = 0; k < Constants.XMAX; k++)
            //{
            //    for (int l = 0; l < Constants.YMAX; l++)
            //    {
            //        Console.Write(board[k, l] + "\t");
            //    }
            //    Console.Write("\n");
            //}
            //Console.Read();
            //Console.Read();
        }

        private int findLength(Coordinate start, Coordinate end)
        {
            if (start.x - end.x != 0)
            {
                return ((start.x - end.x) < 0 ? end.x - start.x : start.x - end.x) + 1;
            }
            return ((start.y - end.y) < 0 ? end.y - start.y : start.y - end.y) + 1;
        }

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
    }
}
