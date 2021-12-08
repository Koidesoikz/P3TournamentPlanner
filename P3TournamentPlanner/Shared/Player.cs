using System;
using System.ComponentModel.DataAnnotations;

namespace P3TournamentPlanner.Shared {
    public class Player {
        public int clubID { get; set; }
        public Nullable<int> teamID { get; set; }

        [Required(ErrorMessage = "Navn er påkrævet")]
        public string IRLName { get; set; }
        public string IGName { get; set; }
        public string steamID { get; set; }
        public string CSGORank { get; set; }
        public int playerSkllRating { get; set; }
        public int playerID { get; set; }

        public Player(string IRLName, string IGName, string steamID, string CSGORank, int playerSkllRating) {
            this.IRLName = IRLName;
            this.IGName = IGName;
            this.steamID = steamID;
            this.CSGORank = CSGORank;
            this.playerSkllRating = playerSkllRating;
        }

        public Player() {
        }

        public Player(string iRLName, string iGName, string steamID, string cSGORank, int playerSkllRating, int playerID) : this(iRLName, iGName, steamID, cSGORank, playerSkllRating) {
            this.playerID = playerID;
        }

        public int CalculateSkillRating() {
            switch(this.CSGORank) {
                case "Silver 1":
                    return playerSkllRating = 4;

                case "Silver 2":
                    return playerSkllRating = 8;

                case "Silver 3":
                    return playerSkllRating = 12;

                case "Silver 4":
                    return playerSkllRating = 17;

                case "Silver Elite":
                    return playerSkllRating = 23;

                case "Silver Elite Master":
                    return playerSkllRating = 31;

                case "Gold Nova 1":
                    return playerSkllRating = 40;

                case "Gold Nova 2":
                    return playerSkllRating = 49;

                case "Gold Nova 3":
                    return playerSkllRating = 58;

                case "Gold Nova Master":
                    return playerSkllRating = 66;

                case "Master Guardian 1":
                    return playerSkllRating = 74;

                case "Master Guardian 2":
                    return playerSkllRating = 81;

                case "Master Guardian Elite":
                    return playerSkllRating = 86;

                case "Distingquished Master Guardian":
                    return playerSkllRating = 90;

                case "Legendary Eagle":
                    return playerSkllRating = 93;

                case "Legendary Eagle Master":
                    return playerSkllRating = 96;

                case "Supreme Master First Class":
                    return playerSkllRating = 99;

                case "The Global Elite":
                    return playerSkllRating = 100;

                default:
                    return 0;
            }
        }
    }
}
