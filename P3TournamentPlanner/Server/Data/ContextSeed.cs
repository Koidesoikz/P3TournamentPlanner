using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using P3TournamentPlanner.Server.Models;
using P3TournamentPlanner.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace P3TournamentPlanner.Server.Data {
    public class ContextSeed {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("Administrator"));
            await roleManager.CreateAsync(new IdentityRole("SuperAdministrator"));
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "Lasse.Kjaer@dgi.dk",
                Email = "Lasse.Kjaer@dgi.dk",
            };

            if(userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Password");
                    await userManager.AddToRoleAsync(defaultUser, "Administrator");
                    await userManager.AddToRoleAsync(defaultUser, "SuperAdministrator");
                    SeedContactInfoDatabaseAsync(defaultUser);
                }
            }

            DatabaseQuerys db = new();
            DataTable dt = new();
            SqlCommand command = new SqlCommand($"select * from ClubDB");
            dt = db.PullTable(command);
            
            if(dt.Rows.Count == 0) {
                await SeedDatabaseForUserTests(userManager, defaultUser);
            }
            
        }
        public static void SeedContactInfoDatabaseAsync(ApplicationUser defaultUser) {
            DatabaseQuerys db = new DatabaseQuerys();
            SqlCommand command = new SqlCommand($"insert into ContactInfoDB(userID, contactName, tlfNumber, discordID, email) values (@userId, @contactName, @tlfNumber, @discordID, @email)");
            command.Parameters.Add(new SqlParameter("userID", defaultUser.Id));
            command.Parameters.Add(new SqlParameter("contactName", "Lasse Kjær"));
            command.Parameters.Add(new SqlParameter("tlfNumber", "87463527"));
            command.Parameters.Add(new SqlParameter("discordID", "Mufasa#6846"));
            command.Parameters.Add(new SqlParameter("email", "Lasse.Kjaer@dgi.dk"));
            db.InsertToTable(command);
        }

        public static async Task SeedDatabaseForUserTests(UserManager<ApplicationUser> userManager, ApplicationUser defaultUser) {
            DatabaseQuerys db = new();
            SqlCommand command;
            List<ClubManager> Managers = new();
            List<Club> Clubs = new();
            List<Team> teams = new();

            Clubs.Add(new Club("Støvring Esport", "Grangårdscentret 27, 9530 Støvring"));
            Clubs.Add(new Club("Ducklings Esport", "Grundtvigsvej 38, Elling 9900 Frederikshavn"));
            Clubs.Add(new Club("Esport Vesthimmerland", "Bymidten 7, 9600 Aars"));
            Clubs.Add(new Club("LKB Esport", "Hadsundvej 407, 9260 Gistrup"));
            Clubs.Add(new Club("Hobro Vikings", "Amerikavej 22 9500 Hobro"));
            Clubs.Add(new Club("Dall Villaby", "Klokkevej 49 9230 Svenstrup J"));

            //int clubID, int divisionID, int leagueID, string teamName, ClubManager manager, Player players

            //string IRLName, string IGName, string steamID, string CSGORank

            //Støvring
            var user = new ApplicationUser {
                UserName = "Rasmus@email.dk",
                Email = "Rasmus@email.dk",
            };
            await userManager.CreateAsync(user, "123Password");
            Managers.Add(new ClubManager(new Contactinfo(user.Id, "Rasmus", "42928808", "Hafer#4918", "Rasmus@email.dk"), user.Id, 1));
            teams.Add(new Team(1, 0, 1, "Støvring Esport", Managers[0], new Player("Emil", "xEmilx", "STEAMID", "Gold Nova Master")));

            //Ducklings
            user = new ApplicationUser {
                UserName = "Benny@email.dk",
                Email = "Benny@email.dk",
            };
            await userManager.CreateAsync(user, "123Password");
            Managers.Add(new ClubManager(new Contactinfo(user.Id, "Benny", "22165430", "Benny The Viking#1203", "Benny@email.dk"), user.Id, 2));
            teams.Add(new Team(2, 0, 1, "Ducklings Esport", Managers[1], new Player("Martin", "xMartinx", "STEAMID", "Gold Nova 1")));

            //vesthimmerland
            user = new ApplicationUser {
                UserName = "Louise@email.dk",
                Email = "Louise@email.dk",
            };
            await userManager.CreateAsync(user, "123Password");
            Managers.Add(new ClubManager(new Contactinfo(user.Id, "Louise", "28783870", "Louise#1234", "Louise@email.dk"), user.Id, 3));
            teams.Add(new Team(3, 0, 1, "Esport Vesthimmerland", Managers[2], new Player("Julian", "xJulianx", "STEAMID", "Silver 3")));

            //LKB
            user = new ApplicationUser {
                UserName = "Henrik@email.dk",
                Email = "Henrik@email.dk",
            };
            await userManager.CreateAsync(user, "123Password");
            Managers.Add(new ClubManager(new Contactinfo(user.Id, "Henrik", "22808246", "zerovco#1662", "Henrik@email.dk"), user.Id, 4));
            teams.Add(new Team(4, 0, 1, "LKB Esport", Managers[3], new Player("Christoffer", "xChrisx", "STEAMID", "Master Guardian 1")));

            //Hobro
            user = new ApplicationUser {
                UserName = "Martin@email.dk",
                Email = "Martin@email.dk",
            };
            await userManager.CreateAsync(user, "123Password");
            Managers.Add(new ClubManager(new Contactinfo(user.Id, "Martin", "23276132", "mrsvanholm#0007", "Martin@email.dk"), user.Id, 5));
            teams.Add(new Team(5, 0, 1, "Hobro Vikings", Managers[4], new Player("Jeppe", "xJeppex", "STEAMID", "Gold Nova 1")));

            //Dall
            user = new ApplicationUser {
                UserName = "Lars@email.dk",
                Email = "Lars@email.dk",
            };
            await userManager.CreateAsync(user, "123Password");
            Managers.Add(new ClubManager(new Contactinfo(user.Id, "Lars", "60507508", "Lars / Sucram#4667", "Lars@email.dk"), user.Id, 6));
            teams.Add(new Team(6, 0, 1, "Dall Villaby Esport", Managers[5], new Player("Arthur", "xArthurx", "STEAMID", "Silver 1")));

            foreach(Club c in Clubs) {
                command = new SqlCommand($"insert into ClubDB(clubName, clubAddress, clubDescription) values (@clubName, @clubAddress, @clubDescription)");
                command.Parameters.Add(new SqlParameter("clubName", c.name));
                command.Parameters.Add(new SqlParameter("clubAddress", c.address));
                command.Parameters.Add(new SqlParameter("clubDescription", "Dette er en beskrivelse"));
                db.InsertToTable(command);
            }

            foreach(ClubManager cm in Managers) {
                command = new SqlCommand($"insert into ClubManagerDB(clubID, userID) values (@clubID, @userID)");
                command.Parameters.Add(new SqlParameter("clubID", cm.ClubID));
                command.Parameters.Add(new SqlParameter("userID", cm.userID));
                db.InsertToTable(command);

                command = new SqlCommand($"insert into ContactInfoDB(userID, contactName, tlfNumber, discordID, email) values (@userID, @contactName, @tlfNumber, @discordID, @email)");
                command.Parameters.Add(new SqlParameter("userID", cm.userID));
                command.Parameters.Add(new SqlParameter("contactName", cm.contactinfo.name));
                command.Parameters.Add(new SqlParameter("tlfNumber", cm.contactinfo.tlfNr));
                command.Parameters.Add(new SqlParameter("discordID", cm.contactinfo.discordID));
                command.Parameters.Add(new SqlParameter("email", cm.contactinfo.email));
                db.InsertToTable(command);
            }


            command = new SqlCommand($"insert into LeagueDB(leagueName, game, adminID, archiveFlag) values(@name, @game, @admin, @flag)");
            command.Parameters.Add(new SqlParameter("name", "Nordjysk Liga"));
            command.Parameters.Add(new SqlParameter("game", "cs:go"));
            command.Parameters.Add(new SqlParameter("admin", defaultUser.Id));
            command.Parameters.Add(new SqlParameter("flag", "0"));
            db.InsertToTable(command);

            command = new SqlCommand("insert into DivisionsDB(divisionID, leagueID, divisionFormat, archiveFlag) values(@divisionID, @leagueID, @divisionFormat, @archiveFlag)");
            command.Parameters.Add(new SqlParameter("divisionID", Convert.ToInt64(0)));
            command.Parameters.Add(new SqlParameter("leagueID", Convert.ToInt32(1)));
            command.Parameters.Add(new SqlParameter("divisionFormat", "holdingDivision"));
            command.Parameters.Add(new SqlParameter("archiveFlag", Convert.ToInt32(0)));
            db.InsertToTable(command);


            int i = 1;
            foreach(Team t in teams) {

                command = new SqlCommand("insert into PlayerDB(teamID, clubID, IRLName, IGName, steamID, csgoRank, skillRating) values (@teamID, @clubID, @IRLName, @IGName, @steamID, @csgoRank, @skillRating)");
                command.Parameters.Add(new SqlParameter("teamID", i));
                command.Parameters.Add(new SqlParameter("clubID", t.clubID));
                command.Parameters.Add(new SqlParameter("IRLName", t.players[0].IRLName));
                command.Parameters.Add(new SqlParameter("IGName", t.players[0].IGName));
                command.Parameters.Add(new SqlParameter("steamID", t.players[0].steamID));
                command.Parameters.Add(new SqlParameter("csgoRank", t.players[0].CSGORank));
                command.Parameters.Add(new SqlParameter("skillRating", t.players[0].CalculateSkillRating()));
                db.InsertToTable(command);
                i++;

                command = new SqlCommand("insert into TeamsDB(clubID, divisionID, leagueID, teamName, teamRating, placement, matchPlayed, matchesWon, matchesDraw, matchesLost, roundsWon, roundsLost, points, managerID, archiveFlag) values(@clubID, @divisionID, @leagueID, @teamName, @teamRating, @placement, @matchPlayed, @matchesWon, @matchesDraw, @matchesLost, @roundsWon, @roundsLost, @points, @managerID, @archiveFlag)");
                command.Parameters.Add(new SqlParameter("clubID", t.clubID));
                command.Parameters.Add(new SqlParameter("divisionID", t.divisionID));
                command.Parameters.Add(new SqlParameter("leagueID", t.leagueID));
                command.Parameters.Add(new SqlParameter("teamName", t.teamName));
                command.Parameters.Add(new SqlParameter("placement", Convert.ToInt32(0)));
                command.Parameters.Add(new SqlParameter("matchPlayed", Convert.ToInt32(0)));
                command.Parameters.Add(new SqlParameter("matchesWon", Convert.ToInt32(0)));
                command.Parameters.Add(new SqlParameter("matchesDraw", Convert.ToInt32(0)));
                command.Parameters.Add(new SqlParameter("matchesLost", Convert.ToInt32(0)));
                command.Parameters.Add(new SqlParameter("roundsWon", Convert.ToInt32(0)));
                command.Parameters.Add(new SqlParameter("roundsLost", Convert.ToInt32(0)));
                command.Parameters.Add(new SqlParameter("points", Convert.ToInt32(0)));
                command.Parameters.Add(new SqlParameter("managerID", t.manager.userID));
                command.Parameters.Add(new SqlParameter("archiveFlag", Convert.ToInt32(0)));
                command.Parameters.Add(new SqlParameter("teamRating", t.calculateTeamSkillRating()));
                db.InsertToTable(command);

                //int clubID, int divisionID, int leagueID, string teamName, ClubManager manager, Player players
            }
        }
    }
}
