using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using P3TournamentPlanner.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace P3TournamentPlanner.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        [HttpGet("matches")]
        public List<Match> Get(int? division, int? teamID)
        {
            Console.WriteLine("Get Recieved!");

            DatabaseQuerys db = new DatabaseQuerys();
            Console.WriteLine("div" + division);
            Console.WriteLine("team" + teamID);

            SqlCommand command = new SqlCommand();

            List<Match> matches = new List<Match>();
            List<Team> teams = new List<Team>();

            DataTable dt, teamTable;

            if (division != null && teamID == null)
            {
                command = new SqlCommand("select matchID, divisionID, leagueID, team1ID, team2ID, team1Score, team2Score, " +
                    "startTime, playedFlag, hostClubID, serverIP, map from MatchDB where divisionID = @divisionID");
                command.Parameters.Add(new SqlParameter("divisionID", division));
            }
            else if (teamID != null && division == null)
            {
                command = new SqlCommand("select matchID, divisionID, leagueID, team1ID, team2ID, team1Score, team2Score, " +
                    "startTime, playedFlag, hostClubID, serverIP, map from MatchDB where team1ID = @teamID or team2ID = @teamID");
                command.Parameters.Add(new SqlParameter("teamID", teamID));
            }

            dt = db.PullTable(command);

            foreach (DataRow r in dt.Rows)
            {
                teams = new List<Team>();

                command = new SqlCommand("select clubID, teamName from TeamsDB where teamID = @teamID");
                command.Parameters.Add(new SqlParameter("teamID", (int)r[3]));
                teamTable = db.PullTable(command);

                teams.Add(new Team((int)r[3], (int)teamTable.Rows[0][0], teamTable.Rows[0][1].ToString()));

                command = new SqlCommand("select clubID, teamName from TeamsDB where teamID = @teamID");
                command.Parameters.Add(new SqlParameter("teamID", (int)r[4]));
                teamTable = db.PullTable(command);

                teams.Add(new Team((int)r[4], (int)teamTable.Rows[0][0], teamTable.Rows[0][1].ToString()));

                matches.Add(new Match((int)r[0], (int)r[2], teams, r[7].ToString(), Convert.ToBoolean(r[8]), (int)r[5], (int)r[6], (int)r[9], r[10].ToString(), r[11].ToString()));
            }
            return matches;
        }

        [HttpGet]
        public Match Get(int matchID)
        {
            Console.WriteLine("Get Recieved!" + matchID);

            DatabaseQuerys db = new DatabaseQuerys();

            Match match;
            List<Team> teams = new List<Team>();

            DataTable dt, teamTable;

            SqlCommand command = new SqlCommand("select matchID, divisionID, leagueID, team1ID, team2ID, team1Score, team2Score, " +
                "startTime, playedFlag, hostClubID, serverIP, map from MatchDB where matchID = @matchID");
            command.Parameters.Add(new SqlParameter("matchID", matchID));

            dt = db.PullTable(command);

            Console.WriteLine(dt.Rows[0][3]);

            command = new SqlCommand("select clubID, teamName from TeamsDB where teamID = @teamID");
            command.Parameters.Add(new SqlParameter("teamID", (int)dt.Rows[0][3]));
            teamTable = db.PullTable(command);

            teams.Add(new Team((int)dt.Rows[0][3], (int)teamTable.Rows[0][0], teamTable.Rows[0][1].ToString()));

            command = new SqlCommand("select clubID, teamName from TeamsDB where teamID = @teamID");
            command.Parameters.Add(new SqlParameter("teamID", (int)dt.Rows[0][4]));
            teamTable = db.PullTable(command);

            teams.Add(new Team((int)dt.Rows[0][4], (int)teamTable.Rows[0][0], teamTable.Rows[0][1].ToString()));

            match = new Match((int)dt.Rows[0][2], (int)dt.Rows[0][1], (int)dt.Rows[0][0], teams, dt.Rows[0][7].ToString(), Convert.ToBoolean(dt.Rows[0][8]), (int)dt.Rows[0][5], (int)dt.Rows[0][6], (int)dt.Rows[0][9], dt.Rows[0][10].ToString(), dt.Rows[0][11].ToString());

            Console.WriteLine(match.teams[0].teamName);

            return match;
        }

        [Authorize]
        [HttpPost]
        public void Post(Match match) {
            Console.WriteLine("Post Recieved!");

            DatabaseQuerys db = new DatabaseQuerys();

            SqlCommand command = new SqlCommand("insert into MatchDB(divisionID, leagueID, team1ID, team2ID, team1Score, team2Score, startTime, playedFlag, hostClubID, serverIP) " +
                "values(@matchDivisionID, @leagueID, @team1ID, @team2ID, @team1Score, @team2Score, @startTime, " +
                "@playedFlag, @hostClubID, @serverIP)");
            command.Parameters.Add(new SqlParameter("matchDivisionID", match.divisionID));
            command.Parameters.Add(new SqlParameter("leagueID", match.leagueID));
            command.Parameters.Add(new SqlParameter("team1ID", match.teams[0].teamID));
            command.Parameters.Add(new SqlParameter("team2ID", match.teams[1].teamID));
            command.Parameters.Add(new SqlParameter("team1Score", match.team1Score));
            command.Parameters.Add(new SqlParameter("team2Score", match.team2Score));
            command.Parameters.Add(new SqlParameter("startTime", match.startTime));
            command.Parameters.Add(new SqlParameter("playedFlag", Convert.ToInt32(match.playedFlag)));
            command.Parameters.Add(new SqlParameter("hostClubID", match.clubHostID));
            command.Parameters.Add(new SqlParameter("serverIP", match.serverIP));

            db.InsertToTable(command);
        }

        [Authorize]
        [HttpPost("postMatchList")]
        public IActionResult PostList(List<Match> matchList) {
            Console.WriteLine("Math post recivied");

            DatabaseQuerys db = new DatabaseQuerys();

            foreach (Match match in matchList) {
                SqlCommand command = new SqlCommand("insert into MatchDB(divisionID, leagueID, team1ID, team2ID, team1Score, team2Score, startTime, playedFlag, hostClubID, serverIP) " +
                    "values(@matchDivisionID, @leagueID, @team1ID, @team2ID, @team1Score, @team2Score, @startTime, " +
                    "@playedFlag, @hostClubID, @serverIP)");
                command.Parameters.Add(new SqlParameter("matchDivisionID", match.divisionID));
                command.Parameters.Add(new SqlParameter("leagueID", match.leagueID));
                command.Parameters.Add(new SqlParameter("team1ID", match.teams[0].teamID));
                command.Parameters.Add(new SqlParameter("team2ID", match.teams[1].teamID));
                command.Parameters.Add(new SqlParameter("team1Score", match.team1Score));
                command.Parameters.Add(new SqlParameter("team2Score", match.team2Score));
                command.Parameters.Add(new SqlParameter("startTime", match.startTime));
                command.Parameters.Add(new SqlParameter("playedFlag", Convert.ToInt32(match.playedFlag)));
                command.Parameters.Add(new SqlParameter("hostClubID", match.clubHostID));
                command.Parameters.Add(new SqlParameter("serverIP", match.serverIP));

                db.InsertToTable(command);
            }
            return Ok("Kampe gemt");
        }

        [Authorize]
        [HttpPut]
        public void Put(Match match) {
            Console.WriteLine("Put Recieved! Match");
            Console.WriteLine(match.divisionID);
            DatabaseQuerys db = new DatabaseQuerys();

            SqlCommand command = new SqlCommand("select playedFlag from matchDB where matchID = @matchID");
            command.Parameters.Add(new SqlParameter("matchID", match.matchID));
            DataTable dt = db.PullTable(command);

            //logic check if placement data should change
            if(match.playedFlag && !Convert.ToBoolean(dt.Rows[0][0])) {
                // her når en kamp blev færdig
                updateDivisionStandings(match);
            } else if(Convert.ToBoolean(dt.Rows[0][0])) {
                // her hvis spilled kamp havde forkert resultat
                reverseDivisionStandings(match.matchID);
                updateDivisionStandings(match);
            }

            //put to matchDB
            command = new SqlCommand("update MatchDB set divisionID = @matchDivisionID, leagueID = @leagueID, team1ID = @team1ID, team2ID = @team2ID, " +
                $"team1Score = @team1Score, team2Score = @team2Score, startTime = @startTime, " +
                $"playedFlag = @playedFlag, hostClubID = @hostClubID, serverIP = @serverIP, map = @map where matchID = @matchID");
            command.Parameters.Add(new SqlParameter("matchDivisionID", match.divisionID));
            command.Parameters.Add(new SqlParameter("leagueID", match.leagueID));
            command.Parameters.Add(new SqlParameter("team1ID", match.teams[0].teamID));
            command.Parameters.Add(new SqlParameter("team2ID", match.teams[1].teamID));
            command.Parameters.Add(new SqlParameter("team1Score", match.team1Score));
            command.Parameters.Add(new SqlParameter("team2Score", match.team2Score));
            command.Parameters.Add(new SqlParameter("startTime", match.startTime));
            command.Parameters.Add(new SqlParameter("playedFlag", Convert.ToInt32(match.playedFlag)));
            command.Parameters.Add(new SqlParameter("hostClubID", match.clubHostID));
            command.Parameters.Add(new SqlParameter("serverIP", match.serverIP));
            command.Parameters.Add(new SqlParameter("matchID", match.matchID));
            command.Parameters.Add(new SqlParameter("map", match.map));



            db.InsertToTable(command);
        }

        private void reverseDivisionStandings(int matchID) {
            DatabaseQuerys db = new DatabaseQuerys();
            DataTable matchdt;
            Match match;
            DataTable teamdt;
            List<Team> teams = new List<Team>();
            SqlCommand command;

            command = new SqlCommand("select * from MatchDB where matchID = @matchID");
            command.Parameters.Add(new SqlParameter("matchID", matchID));
            matchdt = db.PullTable(command);
            //pull teams hvis id'er er i dt - DONE
            for(int i = 0; i < 2; i++) {
                command = new SqlCommand("select * from TeamsDB where teamID = @teamID");
                command.Parameters.Add(new SqlParameter("teamID", matchdt.Rows[0][3 + i]));
                teamdt = db.PullTable(command);
                foreach(DataRow r in teamdt.Rows) {
                    teams.Add(new Team((int)r[0], (int)r[11], (int)r[12], (int)r[6], (int)r[7], (int)r[8], (int)r[9], (int)r[10], (int)r[13]));
                }
            }

            match = new Match((int)matchdt.Rows[0][5], (int)matchdt.Rows[0][6]);

            teams[0].roundsWon -= match.team1Score;
            teams[0].roundsLost -= match.team2Score;

            teams[1].roundsWon -= match.team2Score;
            teams[1].roundsLost -= match.team1Score;

            teams[0].matchesPlayed--;
            teams[1].matchesPlayed--;

            if(match.team1Score > match.team2Score) {
                teams[0].matchesWon--;
                teams[1].matchesLost--;

                teams[0].points -= 3;
            } else if(match.team1Score < match.team2Score) {
                teams[0].matchesLost--;
                teams[1].matchesWon--;

                teams[1].points -= 3;
            } else if(match.team1Score == match.team2Score) {
                teams[0].matchesDraw--;
                teams[0].points--;

                teams[1].matchesDraw--;
                teams[1].points--;
            }


            foreach(Team t in teams) {
                command = new SqlCommand("update TeamsDB set matchPlayed = @matchPlayed, matchesWon = @matchesWon, matchesDraw = @matchesDraw, matchesLost = @matchesLost, roundsWon = @roundsWon, roundsLost = @roundsLost, points = @points where teamID = @teamID");
                command.Parameters.Add(new SqlParameter("matchPlayed", t.matchesPlayed));
                command.Parameters.Add(new SqlParameter("matchesWon", t.matchesWon));
                command.Parameters.Add(new SqlParameter("matchesDraw", t.matchesDraw));
                command.Parameters.Add(new SqlParameter("matchesLost", t.matchesLost));
                command.Parameters.Add(new SqlParameter("roundsWon", t.roundsWon));
                command.Parameters.Add(new SqlParameter("roundsLost", t.roundsLost));
                command.Parameters.Add(new SqlParameter("points", t.points));
                command.Parameters.Add(new SqlParameter("teamID", t.teamID));

                db.InsertToTable(command);
            }




            //lav teams objecter - DONE
            //lav match object - DONE
            //reverse udregninger som de er lavet i updateDivisionStandings(); - DONE
            //update TeamsDB - DONE
        }

        private void updateDivisionStandings(Match match) {
            DatabaseQuerys db = new DatabaseQuerys();
            List<Team> teams = new List<Team>();
            //DataTable team1DT = new DataTable();
            //DataTable team2DT = new DataTable();
            SqlCommand command;

            //gets list of teams involved in the match
            foreach(Team t in match.teams) {
                command = new SqlCommand("select * from TeamsDB where teamID = @teamID");
                command.Parameters.Add(new SqlParameter("teamID", t.teamID));
                DataTable tmpTable = db.PullTable(command);

                foreach(DataRow r in tmpTable.Rows) {
                    teams.Add(new Team((int)r[0], (int)r[11], (int)r[12], (int)r[6], (int)r[7], (int)r[8], (int)r[9], (int)r[10], (int)r[13]));
                }
            }
            //ja det er scuffed
            teams[0].roundsWon += match.team1Score;
            teams[0].roundsLost += match.team2Score;

            teams[1].roundsWon += match.team2Score;
            teams[1].roundsLost += match.team1Score;

            teams[0].matchesPlayed++;
            teams[1].matchesPlayed++;

            if(match.team1Score > match.team2Score) {
                teams[0].matchesWon++;
                teams[1].matchesLost++;

                teams[0].points += 3;
            } else if(match.team1Score < match.team2Score) {
                teams[0].matchesLost++;
                teams[1].matchesWon++;

                teams[1].points += 3;
            } else if(match.team1Score == match.team2Score) {
                teams[0].matchesDraw++;
                teams[0].points++;

                teams[1].matchesDraw++;
                teams[1].points++;
            }

            foreach(Team t in teams) {
                command = new SqlCommand("update TeamsDB set matchPlayed = @matchPlayed, matchesWon = @matchesWon, matchesDraw = @matchesDraw, matchesLost = @matchesLost, roundsWon = @roundsWon, roundsLost = @roundsLost, points = @points where teamID = @teamID");
                command.Parameters.Add(new SqlParameter("matchPlayed", t.matchesPlayed));
                command.Parameters.Add(new SqlParameter("matchesWon", t.matchesWon));
                command.Parameters.Add(new SqlParameter("matchesDraw", t.matchesDraw));
                command.Parameters.Add(new SqlParameter("matchesLost", t.matchesLost));
                command.Parameters.Add(new SqlParameter("roundsWon", t.roundsWon));
                command.Parameters.Add(new SqlParameter("roundsLost", t.roundsLost));
                command.Parameters.Add(new SqlParameter("points", t.points));
                command.Parameters.Add(new SqlParameter("teamID", t.teamID));

                db.InsertToTable(command);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("delete/{matchID}")]
        public IActionResult Delete(int matchID) {
            Console.WriteLine("Delete Received!");
            Console.WriteLine(matchID);
            DatabaseQuerys db = new();
            DataTable dt;

            SqlCommand command = new SqlCommand("select playedFlag from MatchDB where matchID = @matchID");
            command.Parameters.Add(new SqlParameter("matchID", matchID));

            dt = db.PullTable(command);

            foreach(DataRow r in dt.Rows) {
                if((int)r[0] != 0) return BadRequest("Match Played");
            }

            command = new("delete from MatchDB where matchID = @matchID");
            command.Parameters.Add(new SqlParameter("matchID", matchID));

            db.DeleteRow(command);
            return Ok("Match deleted");
        }
    }
}