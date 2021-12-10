using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using P3TournamentPlanner.Shared;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net.Http.Json;

namespace P3TournamentPlanner.Client.Pages.Hold
{
    public class NytHoldBase : ComponentBase
    {
        [Parameter]
        public int clubID { get; set; }

        public Club club { get; set; }

        public List<ClubManager> clubManagers = new List<ClubManager>();
        public List<Team> teams = new List<Team>();

        public Player player = new Player();
        public List<Player> players = new List<Player>();

        public Team team = new Team(new ClubManager(new Contactinfo()), new Club(), new List<Player>());
    }
}
