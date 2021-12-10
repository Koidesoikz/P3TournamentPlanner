using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace P3TournamentPlanner.Shared {
    public class Contactinfo {
        public Contactinfo() {

        }

        public Contactinfo(string userID) {
            this.userID = userID ?? throw new ArgumentNullException(nameof(userID));
        }

        public Contactinfo(string userID, string name, string tlfNr, string discordID, string email) {
            this.userID = userID;
            this.name = name;
            this.tlfNr = tlfNr;
            this.discordID = discordID;
            this.email = email;
        }

        public string userID { get; set; }

        [Required(ErrorMessage = "Navn er påkrævet")]
        public string name { set; get; }

        [Required(ErrorMessage = "Telefon nummer er påkrævet")]
        public string tlfNr { set; get; }

        [Required(ErrorMessage = "Discord navn er påkrævet")]
        public string discordID { set; get; }

        [Required(ErrorMessage = "Email er påkrævet")]
        [EmailAddress]
        public string email { set; get; }
    }
}
