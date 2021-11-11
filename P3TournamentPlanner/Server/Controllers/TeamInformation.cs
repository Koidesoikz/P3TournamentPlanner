﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using P3TournamentPlanner.Shared;
using System.Data;
using Microsoft.Data.SqlClient;

namespace P3TournamentPlanner.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TeamInformationController : ControllerBase {

        [HttpGet]
        public List<Team> Get() {
            Console.WriteLine("Get Received!----------------------------------------------------------------------------->");

            DatabaseQuerys db = new DatabaseQuerys();

            List<Team> teamList = new List<Team>();

            DataTable dt;
            DataTable dt2;

            SqlCommand command = new SqlCommand("select teamID, clubID, divisionID, leagueID, teamName, teamRating, placement, matchPlayed, matchesWon, matchesDraw, matchesLost, roundsWon, roundsLost, points, managerID, archiveFlag from TeamsDB");

            dt = db.PullTable(command);

            //dt = db.PullTable("select teamID, clubID, divisionID, leagueID, teamName, teamRating, placement, matchPlayed, matchesWon, matchesDraw, matchesLost, roundsWon, roundsLost, points, managerID, archiveFlag from TeamsDB");
            //0teamID, 1clubID, 2divisionID, 3leagueID, 4teamName, 5teamRating, 6placement, 7matchPlayed, 8matchesWon, 9matchesDraw, 10matchesLost, 11roundsWon, 12roundsLost, 13points, 14managerID, 15archiveFlag
            foreach(DataRow r in dt.Rows) {

                command = new SqlCommand($"select contactName, tlfNumber, discordID, email from ContactInfoDB where userID=@userID");
                command.Parameters.Add(new SqlParameter("userID", r[14]));
                dt2 = db.PullTable(command);

                //DataRow r2 = dt2.Rows[0];
                //Console.WriteLine((string)r2[0]);
                //teamList.Add(new Team((int)r[0], (int)r[1], (int)r[2], (int)r[3], (string)r[4], (int)r[5], (string)r2[0], (int)r[6],
                //    (int)r[7], (int)r[8], (int)r[9], (int)r[10], (int)r[11], (int)r[12], (int)r[13], (string)r[14], (int)r[15]));

                Contactinfo contactInfo = new Contactinfo((string)dt2.Rows[0][0], (string)dt2.Rows[0][1], (string)dt2.Rows[0][2], (string)dt2.Rows[0][3]);
                ClubManager manager = new ClubManager(contactInfo, (string)r[14]);
                teamList.Add(new Team((int)r[0], (int)r[2], (int)r[3], (string)r[4], (int)r[5], (int)r[6], (int)r[7], (int)r[8], (int)r[9], (int)r[10], (int)r[11], (int)r[12], (int)r[13], manager, Convert.ToBoolean(r[15])));

                Console.WriteLine("HEREEE:--->" + (string)r[4]);
            }
            foreach(Team team in teamList) {
                Console.WriteLine("Under mig:");
                Console.WriteLine(team.teamName);
            }
            Console.WriteLine(teamList);

            //Construct dem som objecter, og send listen afsted

            Console.WriteLine("RETURNING SHITE!!!!!");

            return teamList;
        }
    }
}