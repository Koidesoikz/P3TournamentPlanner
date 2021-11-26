﻿using System;
using System.Collections.Generic;

namespace P3TournamentPlanner.Shared {
    public class Club {

        public int clubID { get; set; }
        public string name { get; set; }
        public List<Player> players { get; set; }
        public List<Team> teams { get; set; }
        public List<ClubManager> clubManagers { get; set; }
        public string address { get; set; }
        public string base64Logo { get; set; }

        public Club() {
        }

        public Club(string name) {
            this.name = name;
        }

        public Club(int clubID, string name, string address, string base64Logo) {
            this.clubID = clubID;
            this.name = name;
            this.address = address;
            this.base64Logo = base64Logo;
        }

        public Club(int clubID)
        {
            this.clubID = clubID;
        }

        //Midlertidig
        public string ImageToBase64(string imgPath) {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }

    }
}