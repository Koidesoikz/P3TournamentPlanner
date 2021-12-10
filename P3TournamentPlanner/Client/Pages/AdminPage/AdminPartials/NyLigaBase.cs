using Microsoft.AspNetCore.Components;
using P3TournamentPlanner.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P3TournamentPlanner.Client.Pages.AdminPage.AdminPartials {
    public class NyLigaBase : ComponentBase {
        [Parameter]
        public int leagueID { get; set; }

        public void GameSelect(string gameName) {
            switch (gameName) {
                case"CS:GO":
                case"cs:go":
                    liga.game = new CSGO();
                    break;
            }
            
        }

        public Contactinfo contactinfo { get; set; }
        public List<League> ligas = new List<League>();
        public List<SiteAdmin> admins = new List<SiteAdmin>();
        public List<User> users = new List<User>();

        public League liga = new League(new List<Division>(), new VideoGame(), new SiteAdmin(new Contactinfo()));

    }
}
