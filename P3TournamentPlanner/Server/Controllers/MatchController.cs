﻿using Microsoft.AspNetCore.Mvc;
using P3TournamentPlanner.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace P3TournamentPlanner.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        [HttpGet]
        public List<Match> Get(int division)
        {
            Console.WriteLine("Get Recieved!");


            DatabaseQuerys db = new DatabaseQuerys();

            List<Match> matchList = new List<Match>();
            


            DataTable matchTable, teamTable;

            matchTable = db.PullTable($"select matchID, divisionID, leagueID, team1ID, team2ID, team1Score, team2Score, " +
                $"startTime, playedFlag, hostClubID, serverIP from MatchDB where divisionID = " + division);


            foreach (DataRow r in matchTable.Rows)
            {
                List<Team> teamList = new List<Team>();
                for (int i = 0; i < 2; i++)
                {
                    teamTable = db.PullTable($"select teamID, clubID, divisionID, leagueID, teamName, teamRating, placement, matchPlayed, matchesWon, matchesDraw, " +
                    $"matchesLost, roundsWon, roundsLost, points, managerID, archiveFlag from TeamsDB where teamID = {(int)r[3 + i]}");
                    foreach (DataRow row in teamTable.Rows)
                    {
                        teamList.Add(new Team((int)row[0], (int)row[1], (int)row[2], (int)row[3], row[4].ToString(), (int)row[5], (int)row[6], (int)row[7], (int)row[8],
                            (int)row[9], (int)row[10], (int)row[11], (int)row[12], (int)row[13], row[14].ToString(), (int)row[15]));
                    }

                }

                matchList.Add(new Match((int)r[0], (int)r[1], (int)r[2], teamList, (int)r[5], (int)r[6], r[7].ToString(), (int)r[8], (int)r[9], r[10].ToString())); //do something with results
            }

            Console.WriteLine(matchTable);

            return matchList;
        }

        [HttpPost]
        public void Post(Match match) {
            Console.WriteLine("Post Recieved!");

            string command = $"insert into MatchDB(divisionID, leagueID, team1ID, team2ID, team1Score, team2Score, startTime, playedFlag, hostClubID, serverIP) " +
                $"values({match.divisionID}, {match.leagueID}, {match.teams[0]}, {match.teams[1]}, {match.resultTeam1}, {match.resultTeam2}, {match.startTime}, {match.playedFlag}, {match.clubHostID}, {match.serverIP})";

        }

        //[HttpPut]
        //public Match Put(Match match) {
        //    Console.WriteLine("Put Recieved!");

        //    string command = $"update MatchDB set divisionID, leagueID, team1ID, team2ID, team1Score, team2Score, startTime, playedFlag, hostClubID, serverIP) values(2, 1, 2, 4, 16, 10, 'Dette er Start time', 1, 2, 'Dette er server IP')";

        //    return match;
        //}
    }
}
