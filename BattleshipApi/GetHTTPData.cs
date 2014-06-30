using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BattleshipAI;

namespace BattleshipApi
{
    public class GetHTTPData
    {
        public int boatlength;
        public int x, y;
        public int isvertical;

        public static List<Coordinate> convertToListCoordinate(List<GetHTTPData> data)
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            Constants.noOfBoats = data.Count();
            Constants.XMAX = 5;
            Constants.YMAX = 7;
            Constants.boatLength = new int[Constants.noOfBoats];
            for (int i = 0; i < data.Count(); i++)
            {
                GetHTTPData gd = data.ElementAt(i);
                Constants.boatLength[i] = gd.boatlength;
                coordinates.Add(new Coordinate(gd.x,gd.y));
                if (gd.isvertical == 0)
                {
                    coordinates.Add(new Coordinate(gd.x,gd.y+gd.boatlength-1));
                }
                else
                {
                    coordinates.Add(new Coordinate(gd.x+gd.boatlength-1,gd.y));
                }
            }

            return coordinates;
        }
    }

    
}