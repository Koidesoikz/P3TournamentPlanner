using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using P3TournamentPlanner.Server.Models;
using P3TournamentPlanner.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace P3TournamentPlanner.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ClubManagerController : ControllerBase {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ClubManagerController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public List<ClubManager> Get(int clubID)
        {
            Console.WriteLine("Get Received!");

            DatabaseQuerys db = new DatabaseQuerys();
            DataTable dt1;

            List<ClubManager> clubManagers = new List<ClubManager>();

            SqlCommand command = new SqlCommand("select userID from ClubManagerDB where clubID = @clubID");
            command.Parameters.Add(new SqlParameter("clubID", clubID));
            dt1 = db.PullTable(command);

            foreach (DataRow r in dt1.Rows)
            {
                DataTable dt2;

                Contactinfo contactinfo = new Contactinfo();

                command = new SqlCommand("select contactName, tlfNumber, discordID, email from ContactInfoDB where userID = @userID");
                command.Parameters.Add(new SqlParameter("userID", r[0].ToString()));

                dt2 = db.PullTable(command);

                foreach (DataRow row in dt2.Rows)
                {
                    contactinfo = new Contactinfo((string)r[0], row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString());
                }

                clubManagers.Add(new ClubManager(contactinfo, r[0].ToString(), clubID));
            }
            return clubManagers;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post(ClubManager cm)
        {
            Console.WriteLine("Manger Post Enter");

            DatabaseQuerys db = new DatabaseQuerys();

            DataTable dt;

            SqlCommand command = new SqlCommand("select contactName, tlfNumber, discordID, email from ContactInfoDB");
            dt = db.PullTable(command);

            foreach (DataRow r in dt.Rows)
            {
                if (r[0].ToString() == cm.contactinfo.name)
                {
                    return BadRequest($"Navn: {cm.contactinfo.name} er allerede taget");
                }
                if (r[1].ToString() == cm.contactinfo.tlfNr)
                {
                    return BadRequest($"Telefon nummer: {cm.contactinfo.tlfNr} er allerede taget");
                }
                if (r[2].ToString() == cm.contactinfo.discordID)
                {
                    return BadRequest($"Discord id: {cm.contactinfo.discordID} er allerede taget");
                }
                if (r[3].ToString() == cm.contactinfo.email)
                {
                    return BadRequest($"Email: {cm.contactinfo.email} er allerede taget");
                }
            }

            Console.WriteLine("IM GONNA FUCKING PREEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");

            Console.WriteLine("POOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOST shit");

            var newUser = new ApplicationUser
            {
                UserName = cm.contactinfo.email,
                Email = cm.contactinfo.email
            };

            CreateUser(newUser);

            command = new SqlCommand("insert into ClubManagerDB(clubID, userID) values (@clubID, @userID)");

            command.Parameters.Add(new SqlParameter("clubID", cm.ClubID));
            command.Parameters.Add(new SqlParameter("userID", newUser.Id));

            db.InsertToTable(command);

            command = new SqlCommand("insert into ContactInfoDB(contactName, tlfNumber, discordID, email, userID) values(" +
                "@contactName, @tlfNumber, @discordID, @email, @userID)");

            command.Parameters.Add(new SqlParameter("contactName", cm.contactinfo.name));
            command.Parameters.Add(new SqlParameter("tlfNumber", cm.contactinfo.tlfNr));
            command.Parameters.Add(new SqlParameter("discordID", cm.contactinfo.discordID));
            command.Parameters.Add(new SqlParameter("email", cm.contactinfo.email));
            command.Parameters.Add(new SqlParameter("userID", newUser.Id));

            db.InsertToTable(command);

            return Ok($"Træner {cm.contactinfo.name} gemt");
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put(ClubManager cm)
        {
            Console.WriteLine("Manger Post Enter");

            DatabaseQuerys db = new DatabaseQuerys();

            DataTable dt;

            SqlCommand command = new SqlCommand("select contactName, tlfNumber, discordID, email from ContactInfoDB where userID != @userID");
            command.Parameters.Add(new SqlParameter("userID", cm.userID));
            dt = db.PullTable(command);

            foreach (DataRow r in dt.Rows)
            {
                if (r[0].ToString() == cm.contactinfo.name)
                {
                    return BadRequest($"Navn: {cm.contactinfo.name} er allerede taget");
                }
                if (r[1].ToString() == cm.contactinfo.tlfNr)
                {
                    return BadRequest($"Telefon nummer: {cm.contactinfo.tlfNr} er allerede taget");
                }
                if (r[2].ToString() == cm.contactinfo.discordID)
                {
                    return BadRequest($"Discord id: {cm.contactinfo.discordID} er allerede taget");
                }
                if (r[3].ToString() == cm.contactinfo.email)
                {
                    return BadRequest($"Email: {cm.contactinfo.email} er allerede taget");
                }
            }

            command = new SqlCommand("update ClubManagerDB set clubID = @clubID where userID = @userID");

            command.Parameters.Add(new SqlParameter("clubID", cm.ClubID));
            command.Parameters.Add(new SqlParameter("userID", cm.userID));

            db.InsertToTable(command);

            command = new SqlCommand("update ContactInfoDB set " +
                "contactName = @contactName, " +
                "tlfNumber = @tlfNumber, " +
                "discordID = @discordID, " +
                "email = @email " +
                "where userID = @userID");

            command.Parameters.Add(new SqlParameter("contactName", cm.contactinfo.name));
            command.Parameters.Add(new SqlParameter("tlfNumber", cm.contactinfo.tlfNr));
            command.Parameters.Add(new SqlParameter("discordID", cm.contactinfo.discordID));
            command.Parameters.Add(new SqlParameter("email", cm.contactinfo.email));
            command.Parameters.Add(new SqlParameter("userID", cm.userID));

            db.InsertToTable(command);

            return Ok($"Træner {cm.contactinfo.name} gemt");
        }

        [Authorize]
        [HttpDelete]
        [Route("Delete/{userID}")]
        public void Delete(string userID)
        {
            Console.WriteLine("Delete Received!");

            DatabaseQuerys db = new();

            SqlCommand command = new("delete from ContactInfoDB where userID = @userID");
            command.Parameters.Add(new SqlParameter("userID", userID));

            db.DeleteRow(command);

            command = new("delete from ClubManagerDB where userID = @userID");
            command.Parameters.Add(new SqlParameter("userID", userID));

            db.DeleteRow(command);
        }

        async void CreateUser(ApplicationUser newUser)
        {
            await userManager.CreateAsync(newUser, "123Password");
        }
    }
}
