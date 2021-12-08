using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class LigaerController : ControllerBase {
        [HttpGet]
        public List<League> Get() {
            Console.WriteLine("League Get Recieved!");

            DatabaseQuerys db = new DatabaseQuerys();
            List<League> leagueList = new List<League>();
            DataTable dt;
            int teamAmount = 0;

            //Pulls from database, to .NET datatable
            SqlCommand command = new SqlCommand("select * from LeagueDB");
            dt = db.PullTable(command);
            //dt = db.PullTable("select * from LeagueDB");

            //Creates teamList, based on said data
            //INDEHOLDE MIDLERTIDIG FIX MED GAME, SOM VI SKAL FIXE SENERE :)
            foreach(DataRow r in dt.Rows) {
                command = new SqlCommand($"select teamID from TeamsDB where leagueID = @leagueID");
                command.Parameters.Add(new SqlParameter("leagueID", r[0]));
                DataTable teamTable = db.PullTable(command);

                foreach(DataRow r2 in teamTable.Rows) {
                    teamAmount++;
                }

                command = new SqlCommand($"select * from ContactInfoDB where userID = @userID");
                command.Parameters.Add(new SqlParameter("userID", r[3]));
                DataTable ciTable = db.PullTable(command);

                Contactinfo ci = new Contactinfo((string)r[3], (string)ciTable.Rows[0][1], (string)ciTable.Rows[0][2], (string)ciTable.Rows[0][3], (string)ciTable.Rows[0][4]);
                SiteAdmin admin = new SiteAdmin(ci);

                if(r[2].ToString() == "CS:GO") {
                    leagueList.Add(new League((string)r[1], admin, new CSGO(), (int)r[0], Convert.ToBoolean(r[4]), teamAmount));
                    //leagueList.Add(new League(r[0].ToString(), new CSGO()));
                } else {
                    leagueList.Add(new League((string)r[1], admin, null, (int)r[0], Convert.ToBoolean(r[4]), teamAmount));
                }
            }

            foreach(League l in leagueList) {
                Console.WriteLine(l.name);
            }

            return leagueList;
        }

        [HttpGet("getleague")]
        public League Get(int LeagueID)
        {
            DatabaseQuerys db = new DatabaseQuerys();
            League league = new League();
            DataTable dt;

            SqlCommand command = new SqlCommand($"select * from LeagueDB where leagueID = @leagueID");
            command.Parameters.Add(new SqlParameter("leagueID", LeagueID));

            dt = db.PullTable(command);

            foreach (DataRow row in dt.Rows)
            {
                league.leagueID = (int)row[0];
                league.name = row[1].ToString();

                db = new DatabaseQuerys();
                DataTable dt2;

                command = new SqlCommand($"select * from ContactInfoDB where userID = @userID");
                command.Parameters.Add(new SqlParameter("userID", row[3].ToString()));

                dt2 = db.PullTable(command);

                league.admin = new SiteAdmin(new Contactinfo(
                    dt2.Rows[0][0].ToString(),
                    dt2.Rows[0][1].ToString(),
                    dt2.Rows[0][2].ToString(),
                    dt2.Rows[0][3].ToString(),
                    dt2.Rows[0][4].ToString()
                ));

                league.archiveFlag = Convert.ToBoolean(row[4]);
            }

            return league;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post(League liga) {
            Console.WriteLine("Liga Post Enter");

            DatabaseQuerys db = new DatabaseQuerys();

            DataTable dt;

            SqlCommand command = new SqlCommand("select leagueName from LeagueDB");
            dt = db.PullTable(command);

            foreach (DataRow r in dt.Rows)
            {
                if (r[0].ToString() == liga.name)
                {
                    return BadRequest($"Navn: {liga.name} er allerede taget");
                }
            }

            //string command = $"insert into LeagueDB(leagueName, game, adminID, archiveFlag) values('{liga.name}, {liga.game}, {liga.admin}, {liga.archiveFlag}')";

            //Create League
            command = new SqlCommand($"insert into LeagueDB(leagueName, game, adminID, archiveFlag) values(@name, @game, @admin, @flag)");
            command.Parameters.Add(new SqlParameter("name", liga.name));
            command.Parameters.Add(new SqlParameter("game", liga.game.name));
            command.Parameters.Add(new SqlParameter("admin", liga.admin.contactinfo.userID));
            command.Parameters.Add(new SqlParameter("flag", liga.archiveFlag));

            db.InsertToTable(command);

            //Det her er omegascuffed og fucking ulovligt
            command = new SqlCommand("select leagueID from LeagueDB where leagueName = @leagueName and game = @game and adminID = @adminID and archiveFlag = @archiveFlag");
            command.Parameters.Add(new SqlParameter("leagueName", liga.name));
            command.Parameters.Add(new SqlParameter("game", liga.game.name));
            command.Parameters.Add(new SqlParameter("adminID", liga.admin.contactinfo.userID));
            command.Parameters.Add(new SqlParameter("archiveFlag", liga.archiveFlag));

            dt = db.PullTable(command);

            //Create Holding Division
            command = new SqlCommand("insert into DivisionsDB(divisionID, leagueID, divisionFormat, archiveFlag) values(@divisionID, @leagueID, @divisionFormat, @archiveFlag)");
            command.Parameters.Add(new SqlParameter("divisionID", Convert.ToInt64(0)));
            command.Parameters.Add(new SqlParameter("leagueID", (int)dt.Rows[0][0]));
            command.Parameters.Add(new SqlParameter("divisionFormat", "holdingDivision"));
            command.Parameters.Add(new SqlParameter("archiveFlag", Convert.ToInt32(0)));

            db.InsertToTable(command);

            return Ok($"Liga: {liga.name} gemt");
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put(League liga) {
            Console.WriteLine("Liga Put Enter");

            DatabaseQuerys db = new DatabaseQuerys();

            DataTable dt;

            SqlCommand command = new SqlCommand("select leagueName from LeagueDB where leagueID != @leagueID");
            command.Parameters.Add(new SqlParameter("leagueID", liga.leagueID));
            dt = db.PullTable(command);

            foreach (DataRow r in dt.Rows)
            {
                if (r[0].ToString() == liga.name)
                {
                    return BadRequest($"Navn: {liga.name} er allerede taget");
                }
            }

            //string command = $"update LeagueDB set leagueName = {liga.name}, game = {liga.game}, adminID = {liga.admin}, archiveFlag = {liga.archiveFlag} where LeagueID = {leagueID}";
            command = new SqlCommand($"update LeagueDB set leagueName = @name, game = @game, adminID = @admin, archiveFlag = @flag where LeagueID = @leagueID");
            command.Parameters.Add(new SqlParameter("name", liga.name));
            command.Parameters.Add(new SqlParameter("game", "cs:go"));
            command.Parameters.Add(new SqlParameter("admin", liga.admin.contactinfo.userID));
            command.Parameters.Add(new SqlParameter("flag", liga.archiveFlag));
            command.Parameters.Add(new SqlParameter("leagueID", liga.leagueID));

            db.InsertToTable(command);

            return Ok($"Liga: {liga.name} gemt");
        }

        [Authorize("Administrator")]
        [HttpPut("archive")]
        public void ArchiveLeague([FromBody] League league, [FromHeader] bool archive) {
            DatabaseQuerys db = new DatabaseQuerys();

            //Update LeagueDB
            SqlCommand command = new SqlCommand($"update LeagueDB set archiveFlag = @archiveFlag where leagueID = @leagueID");
            command.Parameters.Add(new SqlParameter("archiveFlag", Convert.ToInt32(archive)));
            command.Parameters.Add(new SqlParameter("leagueID", league.leagueID));

            db.InsertToTable(command);

            //Update DivisionsDB
            command = new SqlCommand("update DivisionsDB set archiveFlag = @archiveFlag where leagueID = @leagueID");
            command.Parameters.Add(new SqlParameter("archiveFlag", Convert.ToInt32(archive)));
            command.Parameters.Add(new SqlParameter("leagueID", league.leagueID));

            db.InsertToTable(command);

            //Update TeamsDB
            command = new SqlCommand("update TeamsDB set archiveFlag = @archiveFlag where leagueID = @leagueID");
            command.Parameters.Add(new SqlParameter("archiveFlag", Convert.ToInt32(archive)));
            command.Parameters.Add(new SqlParameter("leagueID", league.leagueID));

            db.InsertToTable(command);
        }

    }
}
