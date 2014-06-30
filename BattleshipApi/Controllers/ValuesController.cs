using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BattleshipAI;

namespace BattleshipApi.Controllers
{

    public class ValuesController : ApiController
    {
        static List<GetHTTPData> test;
        // GET api/values
        public List<GetHTTPData> Get()
        {
            return test;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public List<Coordinate> Post([FromBody]List<GetHTTPData> data)
        {
            test = data;
            List<Coordinate> boatCoordinates = GetHTTPData.convertToListCoordinate(data);
            PlayGame playGame = new PlayGame();
            List<Coordinate> moves = playGame.playGame(boatCoordinates);
            return moves;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
