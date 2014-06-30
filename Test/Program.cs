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
    class Program
    {

        //[ImportMany(typeof(IBattleshipPlayAI))]
        //List<IBattleshipPlayAI> _playAlgorithms;

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }

        public static String test()
        {
            return "Success";
        }

        void Run()
        {
            //Compose();

            Console.Write("Finding AI algorithms\n");

            

            //int selectedIndex = -1;
            //Console.Write("Algorithms found:\n");
            //for (int i = 0; i < _playAlgorithms.Count(); i++)
            //{
            //    Console.Write(i + ": " + _playAlgorithms[i].levelName() + "\n");
            //}
            //Console.Write("Choose: ");
            //selectedIndex = Convert.ToInt16(Console.ReadLine());
            Constants.XMAX = 5;
            Constants.YMAX = 7;
            Constants.noOfBoats = 6;
            Constants.boatLength = new int[Constants.noOfBoats];
            Constants.boatLength[0] = 1;
            Constants.boatLength[1] = 1;
            Constants.boatLength[2] = 2;
            Constants.boatLength[3] = 2;
            Constants.boatLength[4] = 3;
            Constants.boatLength[5] = 4;

            int r = 0;
            //while (r < 100)
            //{
            //    new BattleshipSetBoardAI_Easy().SetBoard();
            //    Console.Write(r + "\n");
            //    r++;
            //}
            //Console.Write("Done");
            //Console.Read();

            List<Coordinate> lc = new List<Coordinate>();
            lc.Add(new Coordinate(1, 0));
            lc.Add(new Coordinate(1, 0));
            lc.Add(new Coordinate(2, 2));
            lc.Add(new Coordinate(2, 2));
            lc.Add(new Coordinate(1, 3));
            lc.Add(new Coordinate(1, 4));
            lc.Add(new Coordinate(0, 5));
            lc.Add(new Coordinate(1, 5));
            lc.Add(new Coordinate(3, 4));
            lc.Add(new Coordinate(3, 6));
            lc.Add(new Coordinate(4, 1));
            lc.Add(new Coordinate(4, 4));
            


            r = 0;
            int avg = 0;
            int old = 0;

            while (r < 1000)
            {
                old = avg;
                avg += new PlayGame().playGame(/*new BattleshipSetBoardAI_Easy().SetBoard()*/lc);

                r++;
                Console.Write(r + "\n");
                //Console.Write(r +"\t"+(avg-old)+"\n");
            }

            avg = avg / r;

            Console.Write("avg no of turns: " + avg + "\n");
            Console.Read();

        }


        void Compose()
        {
            AssemblyCatalog catalog = new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly());
            CompositionContainer container = new CompositionContainer(catalog);
            container.SatisfyImportsOnce(this);
        }


        //int testAlgo(int selectedIndex)
        //{

        //    int[,] board = new int[Constants.XMAX, Constants.YMAX];
        //    int hitCount = 0;
        //    int noOfturns = 0;
        //    int hitResult = 0;
        //    Coordinate prevHit = null;
        //    int prev = 0;
        //    Boats boats = Boats.none;
        //    int[] noOfHits = { 2, 3, 3, 4, 5 };


        //    List<Coordinate> coordinates = new BattleshipSetBoardAI_Easy().SetBoard();
        //    ConvertListToBoard(coordinates, board);

        //    //for (int i = 0; i < 5; i++)
        //    //{
        //    //    board[1, i+1] = 5;
        //    //}
        //    //for (int i = 0; i < 4; i++)
        //    //{
        //    //    board[7, i + 2] = 4;
        //    //}
        //    //for (int i = 0; i < 3; i++)
        //    //{
        //    //    board[i+6, 0] = 3;
        //    //}
        //    //for (int i = 0; i < 3; i++)
        //    //{
        //    //    board[9, i + 7] = 2;
        //    //}
        //    //for (int i = 0; i < 2; i++)
        //    //{
        //    //    board[2, i+3] = 1;
        //    //}

        //    //for (int i = 0; i < Constants.XMAX; i++)
        //    //{
        //    //    for (int j = 0; j < Constants.YMAX; j++)
        //    //    {
        //    //        Console.Write(board[i, j] + "\t");
        //    //    }
        //    //    Console.Write("\n");
        //    //}

        //    //Console.Read();
            

        //    while (hitCount < 17)
        //    {
        //        prevHit = _playAlgorithms[selectedIndex].GetMove(prevHit, hitResult, boats);
        //        noOfturns++;
        //        //Console.Write("\nPoint returned: " + prevHit.x + "\t" + prevHit.y);

        //        boats = Boats.none;

        //        if (board[prevHit.x, prevHit.y] > 0)
        //        {
        //            //Console.Write("\nHit");
        //            hitCount++;
        //            noOfHits[board[prevHit.x, prevHit.y] - 1]--;
        //            if (noOfHits[board[prevHit.x, prevHit.y] - 1] == 0)
        //            {
        //                boats = (Boats)(board[prevHit.x, prevHit.y] - 1);
        //            }
        //            //Test for boat destruction

        //            //noOfturns++;
        //            hitResult = 1;
        //        }
        //        else
        //        {
        //            //Console.Write("\nMiss");
        //            //noOfturns++;
        //            hitResult = 2;
        //            prev = 0;
        //        }
        //        //Console.Write("Before read");

        //    }


        //    //Console.Write("no of turns: " + noOfturns + "\n");
        //    return noOfturns;

            
        //}

        public void ConvertListToBoard(List<Coordinate> coordinates,int[,] board)
        {
            for (int i = 0; i < coordinates.Count(); i += 2)
            {
                Coordinate c1 = coordinates.ElementAt(i);
                Coordinate c2 = coordinates.ElementAt(i+1);
                char dir = findDirection(c1,c2);
                int len = findLength(c1,c2);

                switch (dir)
                {
                    case 'u':
                            for (int j = 0; j < len; j++)
                            {
                                board[c1.x - j, c1.y] = (i/2)+1;
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
                                board[c1.x , c1.y - j] = (i / 2) + 1;
                            }
                            break;
                    case 'r':
                            for (int j = 0; j < len; j++)
                            {
                                board[c1.x, c1.y + j] = (i / 2) + 1;
                            }
                            break;
                }
            }
        }

        public int findLength(Coordinate start, Coordinate end)
        {
            if (start.x - end.x != 0)
            {
                return ((start.x - end.x) < 0 ? end.x - start.x : start.x - end.x)+1;
            }
            return ((start.y - end.y) < 0 ? end.y - start.y : start.y - end.y)+1;
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
