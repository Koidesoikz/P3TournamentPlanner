using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using P3TournamentPlanner.Server.Models;
using System;
using System.Collections.Generic;
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
    }
}
