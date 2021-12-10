//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using P3TournamentPlanner.Shared;
//using System.Data;

//namespace P3TournamentPlanner.Server.Controllers {
//    [Route("api/[controller]")]
//    [ApiController]
//    public class SingleTeamInformationController : ControllerBase {

//        [HttpGet]
//        public Team Get(int teamID)
//        {
//            Console.WriteLine("Get Received!----------------------------------------------------------------------------->");

//            DatabaseQuerys db = new DatabaseQuerys();

//            Team team = new Team();

//            DataTable dt;
//            DataTable dt2;

//            dt = db.PullTable($"select * from TeamsDB where teamID = {teamID}");
//            //0teamID, 1clubID, 2divisionID, 3leagueID, 4teamName, 5teamRating, 6placement, 7matchPlayed, 8matchesWon, 9matchesDraw, 10matchesLost, 11roundsWon, 12roundsLost, 13points, 14managerID, 15archiveFlag
            
//            foreach (DataRow r in dt.Rows)
//            {
//                dt2 = db.PullTable($"select contactName, tlfNumber, discordID, email from ContactInfoDB where userID='{r[14]}'");

//                Contactinfo contactInfo = new Contactinfo((string)dt2.Rows[0][0], (string)dt2.Rows[0][1], (string)dt2.Rows[0][2], (string)dt2.Rows[0][3]);
//                ClubManager manager = new ClubManager(contactInfo, (string)r[14]);
//                team = new Team((int)r[0], (int)r[2], (string)r[4], (int)r[5], (int)r[6], (int)r[7], (int)r[8], (int)r[9], (int)r[10], (int)r[11], (int)r[12], (int)r[13], manager, Convert.ToBoolean(r[15]));
                
//                Console.WriteLine("HEREEE:--->" + (string)r[4]);
//            }
//            Console.WriteLine(team.teamName);

//            //Construct dem som objecter, og send listen afsted

//            Console.WriteLine("RETURNING SHITE!!!!!");

//            return team;
//        }
//    }
//}