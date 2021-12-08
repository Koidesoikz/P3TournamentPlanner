using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using P3TournamentPlanner.Shared;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace P3TournamentPlanner.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TeamInformationController : ControllerBase {
        [HttpGet("team")]
        public Team Get(int teamID)
        {
            Console.WriteLine("Get Received!");

            DatabaseQuerys db = new DatabaseQuerys();

            DataTable dt;
            DataTable dt2;

            Team team = new Team();

            SqlCommand command;

            command = new SqlCommand("select * from TeamsDB where teamID = @teamID");
            command.Parameters.Add(new SqlParameter("teamID", teamID));


            dt = db.PullTable(command);

            //dt = db.PullTable("select teamID, clubID, divisionID, leagueID, teamName, teamRating, placement, matchPlayed, matchesWon, matchesDraw, matchesLost, roundsWon, roundsLost, points, managerID, archiveFlag from TeamsDB");
            //0teamID, 1clubID, 2divisionID, 3leagueID, 4teamName, 5teamRating, 6placement, 7matchPlayed, 8matchesWon, 9matchesDraw, 10matchesLost, 11roundsWon, 12roundsLost, 13points, 14managerID, 15archiveFlag
            foreach (DataRow r in dt.Rows)
            {
                command = new SqlCommand($"select contactName, tlfNumber, discordID, email from ContactInfoDB where userID=@userID");
                command.Parameters.Add(new SqlParameter("userID", r[14].ToString()));
                dt2 = db.PullTable(command);

                //DataRow r2 = dt2.Rows[0];
                //Console.WriteLine((string)r2[0]);
                //teamList.Add(new Team((int)r[0], (int)r[1], (int)r[2], (int)r[3], (string)r[4], (int)r[5], (string)r2[0], (int)r[6],
                //    (int)r[7], (int)r[8], (int)r[9], (int)r[10], (int)r[11], (int)r[12], (int)r[13], (string)r[14], (int)r[15]));

                Contactinfo contactInfo = new Contactinfo(r[14].ToString(), (string)dt2.Rows[0][0], (string)dt2.Rows[0][1], (string)dt2.Rows[0][2], (string)dt2.Rows[0][3]);
                ClubManager manager = new ClubManager(contactInfo, r[14].ToString());
                team = new Team(
                    (int)r[0], (int)r[1], (int)r[2], (int)r[3], r[4].ToString(), (int)r[5],
                    (int)r[6], (int)r[7], (int)r[8], (int)r[9], (int)r[10], (int)r[11], (int)r[12],
                    (int)r[13], manager, Convert.ToBoolean(r[15])
                );

                Console.WriteLine("HEREEE:--->");
            }

            return team;
        }

        [HttpGet]
        public List<Team> Get(int? clubID, int? divisionID) {
            Console.WriteLine("Get Received!");

            DatabaseQuerys db = new DatabaseQuerys();

            List<Team> teamList = new List<Team>();

            DataTable dt;
            DataTable dt2;
            DataTable playerDt;

            SqlCommand command = new SqlCommand();

            if(clubID == null && divisionID == null)
            {
                command = new SqlCommand("select teamID, clubID, divisionID, leagueID, teamName, teamRating, placement, matchPlayed, matchesWon, matchesDraw, matchesLost, roundsWon, roundsLost, points, managerID, archiveFlag from TeamsDB");
            }
            else if (clubID != null)
            {
                command = new SqlCommand("select teamID, clubID, divisionID, leagueID, teamName, teamRating, placement, matchPlayed, matchesWon, matchesDraw, matchesLost, roundsWon, roundsLost, points, managerID, archiveFlag from TeamsDB where clubID = @clubID");
                command.Parameters.Add(new SqlParameter("clubID", clubID));
            }
            else if (divisionID != null)
            {
                command = new SqlCommand("select teamID, clubID, divisionID, leagueID, teamName, teamRating, placement, matchPlayed, matchesWon, matchesDraw, matchesLost, roundsWon, roundsLost, points, managerID, archiveFlag from TeamsDB where divisionID = @divisionID");
                command.Parameters.Add(new SqlParameter("divisionID", divisionID));
            }

            dt = db.PullTable(command);

            //dt = db.PullTable("select teamID, clubID, divisionID, leagueID, teamName, teamRating, placement, matchPlayed, matchesWon, matchesDraw, matchesLost, roundsWon, roundsLost, points, managerID, archiveFlag from TeamsDB");
            //0teamID, 1clubID, 2divisionID, 3leagueID, 4teamName, 5teamRating, 6placement, 7matchPlayed, 8matchesWon, 9matchesDraw, 10matchesLost, 11roundsWon, 12roundsLost, 13points, 14managerID, 15archiveFlag
            foreach(DataRow r in dt.Rows) {

                command = new SqlCommand($"select contactName, tlfNumber, discordID, email from ContactInfoDB where userID=@userID");
                command.Parameters.Add(new SqlParameter("userID", r[14].ToString()));
                dt2 = db.PullTable(command);

                command = new SqlCommand($"select * from PlayerDB where teamID = @teamID");
                command.Parameters.Add(new SqlParameter("teamID", r[0]));

                playerDt = db.PullTable(command);

                List<Player> playerList = new List<Player>();

                foreach(DataRow pr in playerDt.Rows) {
                    playerList.Add(new Player((string)pr[3], (string)pr[4], (string)pr[5], (string)pr[6], (int)pr[7]));
                }

                //DataRow r2 = dt2.Rows[0];
                //Console.WriteLine((string)r2[0]);
                //teamList.Add(new Team((int)r[0], (int)r[1], (int)r[2], (int)r[3], (string)r[4], (int)r[5], (string)r2[0], (int)r[6],
                //    (int)r[7], (int)r[8], (int)r[9], (int)r[10], (int)r[11], (int)r[12], (int)r[13], (string)r[14], (int)r[15]));

                Contactinfo contactInfo = new Contactinfo((string)r[14], (string)dt2.Rows[0][0], (string)dt2.Rows[0][1], (string)dt2.Rows[0][2], (string)dt2.Rows[0][3]);
                ClubManager manager = new ClubManager(contactInfo, (string)r[14]);
                //teamList.Add(new Team((int)r[0], (int)r[1], (int)r[2], (int)r[3], (string)r[4], (int)r[5], (int)r[6], (int)r[7], (int)r[8], (int)r[9], (int)r[10], (int)r[11], (int)r[12], (int)r[13], manager, Convert.ToBoolean(r[15])));
                teamList.Add(new Team((int)r[0], (int)r[1], (int)r[2], (int)r[3], (string)r[4], (int)r[5], manager, playerList, (int)r[11], (int)r[12], Convert.ToBoolean(r[15]), (int)r[6], (int)r[7], (int)r[8], (int)r[9], (int)r[10], (int)r[13]));


                Console.WriteLine("HEREEE:--->" + (string)r[4]);
            }

            return teamList;
        }

        [HttpGet ("teamTest")]
        public List<Team> GetTeam(int? clubID, int? divisionID)
        {
            Console.WriteLine("Get Received!");

            DatabaseQuerys db = new DatabaseQuerys();

            List<Team> teamList = new List<Team>();

            List<Player> playerList = new List<Player>();

            DataTable dt;
            DataTable dt2;
            DataTable playerDt;

            SqlCommand command = new SqlCommand();

            if (clubID == null && divisionID == null)
            {
                command = new SqlCommand("select teamID, clubID, teamName, managerID from TeamsDB");
            }
            else if (clubID != null)
            {
                command = new SqlCommand("select teamID, clubID, teamName, managerID from TeamsDB where clubID = @clubID");
                command.Parameters.Add(new SqlParameter("clubID", clubID));
            }
            else if (divisionID != null)
            {
                command = new SqlCommand("select teamID, clubID, teamName, managerID from TeamsDB where divisionID = @divisionID");
                command.Parameters.Add(new SqlParameter("divisionID", divisionID));
            }

            dt = db.PullTable(command);

            //dt = db.PullTable("select teamID, clubID, divisionID, leagueID, teamName, teamRating, placement, matchPlayed, matchesWon, matchesDraw, matchesLost, roundsWon, roundsLost, points, managerID, archiveFlag from TeamsDB");
            //0teamID, 1clubID, 2divisionID, 3leagueID, 4teamName, 5teamRating, 6placement, 7matchPlayed, 8matchesWon, 9matchesDraw, 10matchesLost, 11roundsWon, 12roundsLost, 13points, 14managerID, 15archiveFlag
            foreach (DataRow r in dt.Rows)
            {
                command = new SqlCommand($"select contactName, tlfNumber, discordID, email from ContactInfoDB where userID=@userID");
                command.Parameters.Add(new SqlParameter("userID", r[3].ToString()));
                dt2 = db.PullTable(command);

                //DataRow r2 = dt2.Rows[0];
                //Console.WriteLine((string)r2[0]);
                //teamList.Add(new Team((int)r[0], (int)r[1], (int)r[2], (int)r[3], (string)r[4], (int)r[5], (string)r2[0], (int)r[6],
                //    (int)r[7], (int)r[8], (int)r[9], (int)r[10], (int)r[11], (int)r[12], (int)r[13], (string)r[14], (int)r[15]));

                Contactinfo contactInfo = new Contactinfo((string)r[3], (string)dt2.Rows[0][0], (string)dt2.Rows[0][1], (string)dt2.Rows[0][2], (string)dt2.Rows[0][3]);
                ClubManager manager = new ClubManager(contactInfo, r[3].ToString());
                teamList.Add(new Team((int)r[0], (int)r[1], r[2].ToString(), manager));

                Console.WriteLine("HEREEE:--->");
            }

            return teamList;
        }

        [Authorize]
        [HttpPost]
        public void Post(Team team)
        {
            Console.WriteLine("Team Post Received!");

            DatabaseQuerys db = new DatabaseQuerys();
            DataTable dt;

            SqlCommand command = new SqlCommand("insert into TeamsDB(clubID, divisionID, leagueID, teamName, teamRating, placement, matchPlayed, matchesWon, matchesDraw, matchesLost, roundsWon, roundsLost, points, managerID, archiveFlag) values(@clubID, @divisionID, @leagueID, @teamName, @teamRating, @placement, @matchPlayed, @matchesWon, @matchesDraw, @matchesLost, @roundsWon, @roundsLost, @points, @managerID, @archiveFlag)");
            command.Parameters.Add(new SqlParameter("clubID", team.club.clubID));
            command.Parameters.Add(new SqlParameter("divisionID", team.divisionID));
            command.Parameters.Add(new SqlParameter("leagueID", team.leagueID));
            command.Parameters.Add(new SqlParameter("teamName", team.teamName));
            command.Parameters.Add(new SqlParameter("placement", team.placement));
            command.Parameters.Add(new SqlParameter("matchPlayed", team.matchesPlayed));
            command.Parameters.Add(new SqlParameter("matchesWon", team.matchesWon));
            command.Parameters.Add(new SqlParameter("matchesDraw", team.matchesDraw));
            command.Parameters.Add(new SqlParameter("matchesLost", team.matchesLost));
            command.Parameters.Add(new SqlParameter("roundsWon", team.roundsWon));
            command.Parameters.Add(new SqlParameter("roundsLost", team.roundsLost));
            command.Parameters.Add(new SqlParameter("points", team.points));
            command.Parameters.Add(new SqlParameter("managerID", team.manager.userID));
            command.Parameters.Add(new SqlParameter("archiveFlag", Convert.ToInt32(team.archiveFlag)));
            command.Parameters.Add(new SqlParameter("teamRating", team.calculateTeamSkillRating()));

            db.InsertToTable(command);

            command = new SqlCommand("select teamID from TeamsDB where teamName = @teamName");
            command.Parameters.Add(new SqlParameter("teamName", team.teamName));

            dt = db.PullTable(command);

            foreach (Player player in team.players)
            {
                command = new SqlCommand("insert into PlayerDB(teamID, clubID, IRLName, IGName, steamID, csgoRank, skillRating) values(@teamID, @clubID, @IRLname, @IGname, @steamID, @CSGOrank, @skillRating)");
                command.Parameters.Add(new SqlParameter("teamID", (int)dt.Rows[0][0]));
                command.Parameters.Add(new SqlParameter("clubID", team.club.clubID));
                command.Parameters.Add(new SqlParameter("IRLname", player.IRLName));
                command.Parameters.Add(new SqlParameter("IGname", player.IGName));
                command.Parameters.Add(new SqlParameter("steamID", player.steamID));
                command.Parameters.Add(new SqlParameter("CSGOrank", player.CSGORank));
                command.Parameters.Add(new SqlParameter("skillRating", player.CalculateSkillRating()));

                db.InsertToTable(command);
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put(Team team)
        {
            Console.WriteLine("Put Recivied");

            DatabaseQuerys db = new DatabaseQuerys();

            DataTable dt;

            SqlCommand command = new SqlCommand("select teamName from TeamsDB where teamID != @teamID");
            command.Parameters.Add(new SqlParameter("teamID", team.teamID));
            dt = db.PullTable(command);

            foreach (DataRow r in dt.Rows)
            {
                Console.WriteLine("Team naem: " + r[0].ToString());
                if (r[0].ToString() == team.teamName)
                {
                    return BadRequest("Navn allerede taget!");
                }
            }

            db = new DatabaseQuerys();

            command = new SqlCommand("update TeamsDB set clubID = @clubID, divisionID = @divisionID, leagueID = @leagueID, teamName = @teamName, teamRating = @teamRating, " +
                "placement = @placement, matchPlayed = @matchPlayed, matchesWon = @matchesWon, matchesDraw = @matchesDraw, matchesLost = @matchesLost, roundsWon = @roundsWon, " +
                "roundsLost = @roundsLost, points = @points, managerID = @managerID, archiveFlag = @archiveFlag where teamID = @teamID");

            command.Parameters.Add(new SqlParameter("clubID", team.clubID));
            command.Parameters.Add(new SqlParameter("divisionID", team.divisionID));
            command.Parameters.Add(new SqlParameter("leagueID", team.leagueID));
            command.Parameters.Add(new SqlParameter("teamName", team.teamName));
            command.Parameters.Add(new SqlParameter("teamRating", team.calculateTeamSkillRating()));
            command.Parameters.Add(new SqlParameter("placement", team.placement));
            command.Parameters.Add(new SqlParameter("matchPlayed", team.matchesPlayed));
            command.Parameters.Add(new SqlParameter("matchesWon", team.matchesWon));
            command.Parameters.Add(new SqlParameter("matchesDraw", team.matchesDraw));
            command.Parameters.Add(new SqlParameter("matchesLost", team.matchesLost));
            command.Parameters.Add(new SqlParameter("roundsWon", team.roundsWon));
            command.Parameters.Add(new SqlParameter("roundsLost", team.roundsLost));
            command.Parameters.Add(new SqlParameter("points", team.points));
            command.Parameters.Add(new SqlParameter("managerID", team.manager.userID));
            command.Parameters.Add(new SqlParameter("archiveFlag", team.archiveFlag));
            command.Parameters.Add(new SqlParameter("teamID", team.teamID));

            //Console.WriteLine($"TeamSkillRating: {team.teamSkillRating}");

            db.InsertToTable(command);

            return Ok("Gemt");
        }

        [Authorize("Administrator")]
        [HttpPut("archive")]
        public void ArchiveTeam([FromBody] Team team, [FromHeader] bool archive) {
            Console.WriteLine("ARCHIVE HIT!");
            DatabaseQuerys db = new DatabaseQuerys();

            SqlCommand command = new SqlCommand("update TeamsDB set archiveFlag = @archiveFlag where teamID = @teamID");
            command.Parameters.Add(new SqlParameter("archiveFlag", Convert.ToInt32(archive)));
            command.Parameters.Add(new SqlParameter("teamID", team.teamID));

            db.InsertToTable(command);
        }

        [HttpDelete]
        [Route("Delete/{teamID}")]
        public void Delete(int teamID)
        {
            Console.WriteLine("Delete Received!");
            Console.WriteLine(teamID);

            DatabaseQuerys db = new();

            SqlCommand command = new("delete from TeamsDB where teamID = @teamID");
            command.Parameters.Add(new SqlParameter("teamID", teamID));

            db.DeleteRow(command);
        }
    }
}