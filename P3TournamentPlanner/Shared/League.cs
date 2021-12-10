using System.Collections.Generic;

namespace P3TournamentPlanner.Shared {
    public class League {
        public string name { get; set; }
        public List<Division> divisions { get; set; }
        //public List<SiteAdmin> admins { get; set; }
        public SiteAdmin admin { get; set; }
        public VideoGame game { get; set; }

        //Måske
        public int leagueID { get; set; }
        public bool archiveFlag { get; set; }
        public int teamAmount { get; set; }


        public League() {
        }

        public League(string name) {
            this.name = name;
        }
        public League(List<Division> divisions, VideoGame game, SiteAdmin admin) {
            this.divisions = divisions;
            this.game = game;
            this.admin = admin;
        }

        public League(string name, SiteAdmin admin, VideoGame game, int leagueID, bool archiveFlag, int teamAmount) : this(name) {
            this.admin = admin;
            this.game = game;
            this.leagueID = leagueID;
            this.archiveFlag = archiveFlag;
            this.teamAmount = teamAmount;
        }

        //THIS IS USED FOR TEST!!
        public League(List<Division> divisions) {
            this.divisions = divisions;
        }
    }
}
