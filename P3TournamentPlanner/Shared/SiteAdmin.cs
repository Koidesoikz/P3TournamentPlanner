using System;

namespace P3TournamentPlanner.Shared {
    public class SiteAdmin {
        //Login
        public Contactinfo contactinfo { get; set; }

        public SiteAdmin() {

        }

        public SiteAdmin(Contactinfo contactinfo) {
            this.contactinfo = contactinfo ?? throw new ArgumentNullException(nameof(contactinfo));
        }
    }
}
