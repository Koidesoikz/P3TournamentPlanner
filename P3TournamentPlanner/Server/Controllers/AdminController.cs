using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using P3TournamentPlanner.Server.Data;
using P3TournamentPlanner.Server.Models;
using P3TournamentPlanner.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace P3TournamentPlanner.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class AdminController : ControllerBase {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private List<ApplicationUser> appUserList;
        private List<User> userList = new List<User>();

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet("users")]
        public List<User> GetUsersAsync() {
            //List<ApplicationUser> appUserList = userManager.Users.ToList<ApplicationUser>();
            appUserList = userManager.Users.ToList<ApplicationUser>();

            Console.WriteLine("bling blong");
            UpdateUserListAsync().Wait();
            Console.WriteLine("dinmor");

            return userList;
        }

        [HttpGet("genMatches")]
        public List<Match> GenerateMatches(int leagueID, int divisionID) {
            Random rand = new Random();
            Division division = new Division(new List<Team>(), new List<Match>());

            DatabaseQuerys db = new DatabaseQuerys();
            DataTable teamTable;

            SqlCommand command = new SqlCommand("select * from TeamsDB where leagueID = @leagueID and divisionID = @divisionID");
            command.Parameters.Add(new SqlParameter("leagueID", leagueID));
            command.Parameters.Add(new SqlParameter("divisionID", divisionID));
            teamTable = db.PullTable(command);

            foreach(DataRow r in teamTable.Rows) {
                command = new SqlCommand("select * from ContactInfoDB where userID = @userID");
                command.Parameters.Add(new SqlParameter("userID", (string)r[14]));
                DataTable ciTable = db.PullTable(command);

                division.teams.Add(new Team((int)r[0], (int)r[1], (int)r[2], (int)r[3], (string)r[4], (int)r[5], new ClubManager(new Contactinfo((string)ciTable.Rows[0][0], (string)ciTable.Rows[0][1], (string)ciTable.Rows[0][2], (string)ciTable.Rows[0][3], (string)ciTable.Rows[0][4]))));
            }

            //Logic
            for(int i = 0; i < division.teams.Count - 1; i++) {
                for(int j = i + 1; j < division.teams.Count; j++) {
                    List<Team> teamsInMatch = new List<Team>();
                    teamsInMatch.Add(division.teams[i]);
                    teamsInMatch.Add(division.teams[j]);

                    division.matches.Add(new Match(teamsInMatch, divisionID, leagueID, "This is the start time", false, teamsInMatch[rand.Next(0, 2)].clubID, "This is the ServerIP", "This is the map", 0, 0));
                }
            }

            return division.matches;
        }

        [HttpGet("genDivisions")]
        public List<Division> GenerateDivisions(int leagueID, int divisionAmount) {
            List<Division> divisions = new List<Division>();
            List<Team> teamList = new List<Team>();

            //Datacollection
            DatabaseQuerys db = new DatabaseQuerys();
            DataTable dt;

            SqlCommand command = new SqlCommand("select teamID, clubID, teamName, teamRating, managerID from TeamsDB where leagueID = @leagueID and divisionID = 0");
            command.Parameters.Add(new SqlParameter("leagueID", leagueID));
            //command.Parameters.Add(new SqlParameter("divisionID", (bigint)0));
            dt = db.PullTable(command);

            //1teamID, 2clubID, 3teamName, 4teamRating, 5managerID
            foreach(DataRow r in dt.Rows) {
                command = new SqlCommand("select contactName, tlfNumber, discordID, email from ContactInfoDB where userID = @managerID");
                command.Parameters.Add(new SqlParameter("managerID", (string)r[4]));
                DataTable manInfo = db.PullTable(command);

                teamList.Add(new Team((int)r[0], (int)r[1], 0, leagueID, (string)r[2], (int)r[3], 0, 0, 0, 0, 0, 0, 0, 0, new ClubManager(new Contactinfo((string)r[4], (string)manInfo.Rows[0][0], (string)manInfo.Rows[0][1], (string)manInfo.Rows[0][2], (string)manInfo.Rows[0][3]), (string)r[4]), false));
            }

            //Sorts the teams in decending order, based on the skill rating
            teamList.Sort((x, y) => x.teamSkillRating.CompareTo(y.teamSkillRating));

            //Division Generation
            int teamsLeft = teamList.Count();

            foreach(Team t in teamList) {
                Console.WriteLine(t.teamName);
            }

            for(int i = 0; i < divisionAmount; i++) {
                divisions.Add(new Division(i + 1, new List<Team>(), leagueID, false));
                Console.WriteLine("Teams left pre: " + teamsLeft);
                Console.WriteLine($"Division left pre: {divisionAmount - i}");
                Console.WriteLine($"There are {Math.Ceiling(((float)teamsLeft / ((float)divisionAmount - i)))} teams in division {i + 1}");
                for(int j = 0; j < Math.Ceiling(((float)teamsLeft / ((float)divisionAmount - i))); j++) {
                    Console.WriteLine(teamList[teamsLeft - 1 - j].teamName);
                    divisions[i].teams.Add(teamList[teamsLeft - 1 - j]);
                }

                teamsLeft -= (int)Math.Ceiling(((float)teamsLeft / ((float)divisionAmount - i)));
                Console.WriteLine("");
            }

            return divisions;
        }

        [Authorize(Roles = "SuperAdministrator")]
        [HttpPost("changeRole")]
        public IActionResult PostRole([FromBody] Contactinfo contactinfo) {
            Console.WriteLine("Manger Post Enter");

            DatabaseQuerys db = new DatabaseQuerys();

            DataTable dt;

            SqlCommand command = new SqlCommand("select contactName, tlfNumber, discordID, email from ContactInfoDB");
            dt = db.PullTable(command);

            foreach (DataRow r in dt.Rows)
            {
                if (r[0].ToString() == contactinfo.name)
                {
                    return BadRequest($"Navn: {contactinfo.name} er allerede taget");
                }
                if (r[1].ToString() == contactinfo.tlfNr)
                {
                    return BadRequest($"Telefon nummer: {contactinfo.tlfNr} er allerede taget");
                }
                if (r[2].ToString() == contactinfo.discordID)
                {
                    return BadRequest($"Discord id: {contactinfo.discordID} er allerede taget");
                }
                if (r[3].ToString() == contactinfo.email)
                {
                    return BadRequest($"Email: {contactinfo.email} er allerede taget");
                }
            }

            ApplicationUser newUser = new ApplicationUser();
            newUser.Email = contactinfo.email;
            newUser.UserName = contactinfo.email;

            CreateUser(newUser);

            command = new SqlCommand($"insert into ContactInfoDB(userID, contactName, tlfNumber, discordID, email) values (@userId, @contactName, @tlfNumber, @discordID, @email)");
            command.Parameters.Add(new SqlParameter("userID", newUser.Id));
            command.Parameters.Add(new SqlParameter("contactName", contactinfo.name));
            command.Parameters.Add(new SqlParameter("tlfNumber", contactinfo.tlfNr));
            command.Parameters.Add(new SqlParameter("discordID", contactinfo.discordID));
            command.Parameters.Add(new SqlParameter("email", contactinfo.email));
            db.InsertToTable(command);

            //Console.WriteLine("PUT ENTERED!!!!!!!");

            //Console.WriteLine("bool: " + toBecomeAdmin);

            //Console.WriteLine("USERID!!!: " + user.ID);

            //ApplicationUser appUser = await userManager.FindByIdAsync(user.ID);

            //if(toBecomeAdmin) {
            //    await userManager.AddToRoleAsync(appUser, "Administrator");
            //} else {
            //    await userManager.RemoveFromRoleAsync(appUser, "Administrator");
            //}
            return Ok($"Administrator: {contactinfo.name} oprettet");
        }

        public async Task UpdateUserListAsync() {
            foreach(ApplicationUser appUser in appUserList) {
                List<string> rolesList = new List<string>();
                var roles = await userManager.GetRolesAsync(appUser);
                foreach(var role in roles) {
                    Console.WriteLine("Lookie: " + role.ToString());
                    rolesList.Add(role.ToString());
                }
                userList.Add(new User(appUser.Id, appUser.UserName, rolesList));
            }
        }

        async void CreateUser(ApplicationUser newUser)
        {
            await userManager.CreateAsync(newUser, "123Password");
            await userManager.AddToRoleAsync(newUser, "Administrator");
        }
    }
}
