using System.Collections.Generic;

namespace P3TournamentPlanner.Shared {
    public class User {
        public User() {
        }

        public User(string ID, string email, List<string> roles) {
            this.ID = ID;
            this.email = email;
            this.roles = roles;
        }

        public string ID { get; set; }
        public string email { get; set; }
        public List<string> roles { get; set; }
    }
}
