using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoundRobin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P3TournamentPlanner.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RoundRobinController : ControllerBase {
        [HttpGet]
        public List<int> Get(List<int> teams)
        {
            Console.WriteLine("Get Received for Round Robin!");
            var roundRobinList = new RoundRobinList<int>(teams);

            List<int> roundRobin = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                roundRobin.Add(roundRobinList.Next());
            }

            return roundRobin;
        }
    }
}
