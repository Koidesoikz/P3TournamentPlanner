using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P3TournamentPlanner.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace P3TournamentPlanner.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase {


        [HttpGet]
        public List<Player> Get(int? teamID, int? clubID) {
            Console.WriteLine("Player Get Recieved!");
            Console.WriteLine("teamID" + teamID);
            Console.WriteLine("clubID" + clubID);

            DatabaseQuerys db = new DatabaseQuerys();
            List<Player> playerList = new List<Player>();
            DataTable dt;

            SqlCommand command = new SqlCommand();

            if (teamID != null)
            {
                command = new SqlCommand("select playerID, teamID, clubID, IRLName, IGName, steamID, csgoRank, skillRating from PlayerDB where teamID = @teamID");
                command.Parameters.Add(new SqlParameter("teamID", teamID));
            }
            else if (clubID != null)
            {
                command = new SqlCommand("select playerID, teamID, clubID, IRLName, IGName, steamID, csgoRank, skillRating from PlayerDB where clubID = @clubID");
                command.Parameters.Add(new SqlParameter("clubID", clubID));
            }

            dt = db.PullTable(command);

            //0teamID, 1clubID, 2IRLName, 3IGName, 4steamID, 5csgoRank

            foreach(DataRow r in dt.Rows) {
                playerList.Add(new Player((string)r[3], (string)r[4], (string)r[5], (string)r[6], (int)r[7], (int)r[0]));

                //playerList.Add(new Player((string)r[2], (string)r[3], (string)r[4], (string)r[5], (int)r[6]));

                //playerList.Add(new Player((int)r[1], (int)r[0], r[2].ToString(), r[3].ToString(), r[4].ToString(), r[5].ToString()));
            }

            foreach(Player p in playerList) {
                Console.WriteLine(p.IRLName);
            }

            return playerList;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post(Player player) {
            Console.WriteLine("Player Post Enter");

            DatabaseQuerys db = new DatabaseQuerys();

            DataTable dt;

            SqlCommand command = new SqlCommand("select steamID from playerDB where steamID != @steamID");
            command.Parameters.Add(new SqlParameter("steamID", "Indsæt STEAM ID"));
            dt = db.PullTable(command);

            foreach (DataRow r in dt.Rows)
            {
                if (r[0].ToString() == player.steamID && player.steamID != "Indsæt STEAM ID")
                {
                    return BadRequest($"Steam ID: {player.steamID} er allerede taget");
                }
            }

            if (player.teamID != null) {
                command = new SqlCommand("insert into PlayerDB(teamID, clubID, IRLName, IGName, steamID, csgoRank, skillRating) values (@teamID, @clubID, @IRLName, @IGName, @steamID, @csgoRank, @skillRating)");
            } else {
                command = new SqlCommand("insert into PlayerDB(clubID, IRLName, IGName, steamID, csgoRank, skillRating) values (@clubID, @IRLName, @IGName, @steamID, @csgoRank, @skillRating)");
            }

            if(player.teamID != null) command.Parameters.Add(new SqlParameter("teamID", player.teamID));
            command.Parameters.Add(new SqlParameter("clubID", player.clubID));
            command.Parameters.Add(new SqlParameter("IRLName", player.IRLName));
            command.Parameters.Add(new SqlParameter("IGName", player.IGName));
            command.Parameters.Add(new SqlParameter("steamID", player.steamID));
            command.Parameters.Add(new SqlParameter("csgoRank", player.CSGORank));
            command.Parameters.Add(new SqlParameter("skillRating", player.CalculateSkillRating()));

            db.InsertToTable(command);
            
            Console.WriteLine("Post Done");
            return Ok("Oprettet");
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put(Player player) {
            Console.WriteLine("Player Put Enter");

            DatabaseQuerys db = new DatabaseQuerys();

            DataTable dt;

            SqlCommand command = new SqlCommand("select steamID from playerDB where playerID != @playerID and steamID != @steamID");
            command.Parameters.Add(new SqlParameter("playerID", player.playerID));
            command.Parameters.Add(new SqlParameter("steamID", "Indsæt STEAM ID"));
            dt = db.PullTable(command);

            foreach (DataRow r in dt.Rows)
            {
                if (r[0].ToString() == player.steamID)
                {
                    return BadRequest($"Steam ID: {player.steamID} er allerede taget");
                }
            }

            command = new SqlCommand("use GeneralDatabase update PlayerDB set teamID = @teamID, clubID = @clubID, IRLName = @IRLName, IGName = @IGName, steamID = @steamID, csgoRank = @csgoRank, skillRating = @skillRating where playerID = @playerID");
            
            command.Parameters.Add(new SqlParameter("teamID", player.teamID));
            command.Parameters.Add(new SqlParameter("clubID", player.clubID));
            command.Parameters.Add(new SqlParameter("IRLName", player.IRLName));
            command.Parameters.Add(new SqlParameter("IGName", player.IGName));
            command.Parameters.Add(new SqlParameter("steamID", player.steamID));
            command.Parameters.Add(new SqlParameter("csgoRank", player.CSGORank));
            command.Parameters.Add(new SqlParameter("skillRating", player.CalculateSkillRating()));
            command.Parameters.Add(new SqlParameter("playerID", player.playerID));

            db.InsertToTable(command);
            return Ok("Gemt");
        }

        [HttpDelete]
        [Route("Delete/{playerID}")]
        public void Delete(int playerID)
        {
            Console.WriteLine("Delete Received!");

            DatabaseQuerys db = new();

            SqlCommand command = new("delete from PlayerDB where playerID = @playerID");
            command.Parameters.Add(new SqlParameter("playerID", playerID));

            db.DeleteRow(command);
        }
    }
}
