using NUnit.Framework;
using P3TournamentPlanner.Server.Controllers;
using P3TournamentPlanner.Shared;
using System.Collections.Generic;

namespace UnitTest {
    public class Tests {

        //<----------AdminController Unit Tests---------->

        //Test of the "GenerateMatches" function in AdminController
        private static readonly object[] genMatchSource = {
            new object[] {
                1,
                1,
                true,
                new Division {
                        divisionID = 1,
                        teams = new List<Team> {
                            new Team {
                                teamID = 1,
                                clubID = 1,
                                divisionID = 1,
                                leagueID = 1,
                                teamName = "Team1",
                                teamSkillRating = 420,
                                manager = new ClubManager() {
                                    contactinfo = new Contactinfo() {
                                        userID = "Team1ManID",
                                        name = "Team1ManName",
                                        tlfNr = "Team1ManTlf",
                                        discordID = "Team1ManDisc",
                                        email = "Team1ManMail"
                                    }
                                }
                            },
                            new Team {
                                teamID = 2,
                                clubID = 2,
                                divisionID = 1,
                                leagueID = 1,
                                teamName = "Team2",
                                teamSkillRating = 410,
                                manager = new ClubManager() {
                                    contactinfo = new Contactinfo() {
                                        userID = "Team2ManID",
                                        name = "Team2ManName",
                                        tlfNr = "Team2ManTlf",
                                        discordID = "Team2ManDisc",
                                        email = "Team2ManMail"
                                    }
                                }
                            },
                            new Team {
                                teamID = 3,
                                clubID = 1,
                                divisionID = 1,
                                leagueID = 1,
                                teamName = "Team3",
                                teamSkillRating = 405,
                                manager = new ClubManager() {
                                    contactinfo = new Contactinfo() {
                                        userID = "Team3ManID",
                                        name = "Team3ManName",
                                        tlfNr = "Team3ManTlf",
                                        discordID = "Team3ManDisc",
                                        email = "Team3ManMail"
                                    }
                                }
                            }
                        },
                        matches = new List<Match>{}
                        //new Team(1, 1, 1, 1, "Team1", 420, new ClubManager(new Contactinfo("Team1ManID", "Team1ManName", "Team1ManTlf", "Team1ManDisc", "Team1ManMail"))),
                        //new Team(2, 2, 1, 1, "Team2", 410, new ClubManager(new Contactinfo("Team2ManID", "Team2ManName", "Team2ManTlf", "Team2ManDisc", "Team2ManMail"))),
                        //new Team(3, 1, 1, 1, "Team3", 405, new ClubManager(new Contactinfo("Team3ManID", "Team3ManName", "Team3ManTlf", "Team3ManDisc", "Team3ManMail")))

                        },
                3
            },
            new object[] {
                1,
                1,
                true,
                new Division {
                        divisionID = 2,
                        teams = new List<Team> {
                            new Team {
                                teamID = 4,
                                clubID = 1,
                                divisionID = 2,
                                leagueID = 1,
                                teamName = "Team4",
                                teamSkillRating = 400,
                                manager = new ClubManager() {
                                    contactinfo = new Contactinfo() {
                                        userID = "Team4ManID",
                                        name = "Team4ManName",
                                        tlfNr = "Team4ManTlf",
                                        discordID = "Team4ManDisc",
                                        email = "Team4ManMail"
                                    }
                                }
                            },
                            new Team {
                                teamID = 5,
                                clubID = 2,
                                divisionID = 2,
                                leagueID = 1,
                                teamName = "Team5",
                                teamSkillRating = 390,
                                manager = new ClubManager() {
                                    contactinfo = new Contactinfo() {
                                        userID = "Team5ManID",
                                        name = "Team5ManName",
                                        tlfNr = "Team5ManTlf",
                                        discordID = "Team5ManDisc",
                                        email = "Team5ManMail"
                                    }
                                }
                            }
                        },
                        matches = new List<Match>{}
                        //new Team(4, 1, 2, 1, "Team4", 400, new ClubManager(new Contactinfo("Team4ManID", "Team4ManName", "Team4ManTlf", "Team4ManDisc", "Team4ManMail"))),
                        //new Team(5, 2, 2, 1, "Team5", 390, new ClubManager(new Contactinfo("Team5ManID", "Team5ManName", "Team5ManTlf", "Team5ManDisc", "Team5ManMail")))

                    },
                1
            }
        };

        [TestCaseSource("genMatchSource"), Category("AdminController Test")]
        public void GenMatchTest(int leagueID, int divisionID, bool testing, Division testDiv,int expectedMatchAmount) {
            var sut = new AdminController();
            List<Match> matchList = sut.GenerateMatches(leagueID, divisionID, testing, testDiv);
            Assert.That(matchList.Count, Is.EqualTo(expectedMatchAmount));
        }

        //Test of the "GenerateDivisions" function in AdminController
        private static readonly object[] genDivSource = {
            new object[] {
                1,
                2,
                true,
                new List<Team> {
                    new Team {
                        teamID = 1,
                        clubID = 1,
                        teamName = "Team1",
                        teamSkillRating = 420,
                        manager = new ClubManager {
                            contactinfo = new Contactinfo {
                                userID = "Team1ManID",
                                name = "Team1ManName",
                                tlfNr = "Team1ManTlf",
                                discordID = "Team1ManDisc",
                                email = "Team1ManMail"
                            }
                        }
                    },
                    new Team {
                        teamID = 2,
                        clubID = 2,
                        teamName = "Team2",
                        teamSkillRating = 415,
                        manager = new ClubManager {
                            contactinfo = new Contactinfo {
                                userID = "Team2ManID",
                                name = "Team2ManName",
                                tlfNr = "Team2ManTlf",
                                discordID = "Team2ManDisc",
                                email = "Team2ManMail"
                            }
                        }
                    },
                    new Team {
                        teamID = 3,
                        clubID = 1,
                        teamName = "Team3",
                        teamSkillRating = 400,
                        manager = new ClubManager {
                            contactinfo = new Contactinfo {
                                userID = "Team3ManID",
                                name = "Team3ManName",
                                tlfNr = "Team3ManTlf",
                                discordID = "Team3ManDisc",
                                email = "Team3ManMail"
                            }
                        }
                    },
                    new Team {
                        teamID = 4,
                        clubID = 2,
                        teamName = "Team4",
                        teamSkillRating = 370,
                        manager = new ClubManager {
                            contactinfo = new Contactinfo {
                                userID = "Team4ManID",
                                name = "Team4ManName",
                                tlfNr = "Team4ManTlf",
                                discordID = "Team4ManDisc",
                                email = "Team4ManMail"
                            }
                        }
                    },
                    new Team {
                        teamID = 5,
                        clubID = 1,
                        teamName = "Team5",
                        teamSkillRating = 320,
                        manager = new ClubManager {
                            contactinfo = new Contactinfo {
                                userID = "Team5ManID",
                                name = "Team5ManName",
                                tlfNr = "Team5ManTlf",
                                discordID = "Team5ManDisc",
                                email = "Team5ManMail"
                            }
                        }
                    },
                },
                new List<int> {
                    3,
                    2
                },
                new List<List<string>> {
                    new List<string> {
                        "Team1",
                        "Team2",
                        "Team3"
                    },
                    new List<string> {
                        "Team4",
                        "Team5"
                    }
                }

            }
        };

        [TestCaseSource("genDivSource"), Category("AdminController Test")]
        public void GenDivTest(int leagueID, int divisionAmount, bool testing, List<Team> testTeamList, List<int> expectedTeamAmount, List<List<string>> expectedTeamNames) {
            var sut = new AdminController();

            List<Division> res = sut.GenerateDivisions(leagueID, divisionAmount, testing, testTeamList);

            int count = 0;

            foreach(List<string> sList in expectedTeamNames) {
                Assert.That(res[count].teams.Count, Is.EqualTo(expectedTeamAmount[count]));
                int count2 = 0;
                foreach(string s in sList) {
                    Assert.That(res[count].teams[count2].teamName, Is.EqualTo(s));
                    count2++;
                }
                count++;
            }
        }

        //<----------MatchController Unit Tests---------->

        //Test of the "ReverseDivisionStandings" function in MatchController
        //private static readonly object[] RevDivSource = {
        //    new object[] {
        //        new Match {
        //            team1Score = 16,
        //            team2Score = 5,
        //            teams = new List<Team> {
        //                new Team {
        //                    teamID = 1,
        //                    roundsWon = 35,
        //                    roundsLost = 23,
        //                    placement = 1,
        //                    matchesPlayed = 5,
        //                    matchesWon = 4,
        //                    matchesLost = 1,
        //                    matchesDraw = 0,
        //                    points = 12
        //                },
        //                new Team {
        //                    teamID = 2,
        //                    roundsWon = 25,
        //                    roundsLost = 27,
        //                    placement = 3,
        //                    matchesPlayed = 5,
        //                    matchesWon = 3,
        //                    matchesLost = 1,
        //                    matchesDraw = 1,
        //                    points = 10
        //                }
        //            }
        //        },
        //        new List<Team> {
        //            new Team {
        //                roundsWon = 19,
        //                roundsLost = 18,
        //                matchesPlayed = 4,
        //                matchesWon = 3,
        //                matchesLost = 1,
        //                matchesDraw = 0,
        //                points = 9
        //            },
        //            new Team {
        //                roundsWon = 20,
        //                roundsLost = 11,
        //                matchesPlayed = 4,
        //                matchesWon = 3,
        //                matchesLost = 0,
        //                matchesDraw = 1,
        //                points = 10
        //            }
        //        }
        //    }
        //};

        //public void RevDivTest(Match match, List<Team> excpected) {
        //    var sut = new MatchController();



        //}

        //Test of the "UpdateDivisionStandings" function in MatchController

        //<----------Player Unit Tests---------->

        //Test of the "CalculateSkillRating" function in Player
        private static readonly object[] CalcSkillRatSource = {
            new object[] {
                new Player {
                    CSGORank = "Silver 1" 
                },
                4
            },
            new object[] {
                new Player {
                    CSGORank = "Silver 4"
                },
                17
            },
            new object[] {
                new Player {
                    CSGORank = "Gold Nova 3"
                },
                58
            },
            new object[] {
                new Player {
                    CSGORank = "Master Guardian 1"
                },
                74
            },
            new object[] {
                new Player {
                    CSGORank = "Master Guardian Elite"
                },
                86
            },
            new object[] {
                new Player {
                    CSGORank = "Legendary Eagle Master"
                },
                96
            },
            new object[] {
                new Player {
                    CSGORank = "The Global Elite"
                },
                100
            }
        };

        [TestCaseSource("CalcSkillRatSource"), Category("Player Test")]
        public void CalcSkillRatTest(Player player, int expectedRating) {
            player.CalculateSkillRating();

            Assert.That(player.playerSkllRating, Is.EqualTo(expectedRating));
        }

        //<----------Team Unit Tests---------->

        //Test of the "CalculateTeamSkillRating" function in Team
        private static readonly object[] CalcTeamSkillRatSource = {
            new object[] {
                new List<Player> {
                    new Player {
                        CSGORank = "Silver 4"
                    },
                    new Player {
                        CSGORank = "Silver 4"
                    },
                    new Player {
                        CSGORank = "Silver 2"
                    },
                    new Player {
                        CSGORank = "Silver 2"
                    },
                    new Player {
                        CSGORank = "Silver 3"
                    }
                },
                12
            },
            new object[] {
                new List<Player> {
                    new Player {
                        CSGORank = "The Global Elite"
                    },
                    new Player {
                        CSGORank = "Distingquished Master Guardian"
                    },
                    new Player {
                        CSGORank = "Gold Nova 3"
                    },
                    new Player {
                        CSGORank = "Master Guardian Elite"
                    },
                    new Player {
                        CSGORank = "Legendary Eagle"
                    }
                },
                85
            },
            new object[] {
                new List<Player> {
                    new Player {
                        CSGORank = "The Global Elite"
                    },
                    new Player {
                        CSGORank = "Silver 1"
                    },
                    new Player {
                        CSGORank = "Gold Nova 3"
                    },
                },
                54
            },
            new object[] {
                new List<Player> {
                    new Player {
                        CSGORank = "The Global Elite"
                    },
                    new Player {
                        CSGORank = "Distingquished Master Guardian"
                    },
                    new Player {
                        CSGORank = "Gold Nova 3"
                    },
                    new Player {
                        CSGORank = "Master Guardian Elite"
                    },
                    new Player {
                        CSGORank = "Legendary Eagle"
                    },
                    new Player {
                        CSGORank = "Legendary Eagle"
                    },
                    new Player {
                        CSGORank = "Legendary Eagle"
                    }
                },
                87
            },
            new object[] {
                new List<Player> {
                },
                0
            }
        };

        [TestCaseSource("CalcTeamSkillRatSource"), Category("Team Test")]
        public void CalcTeamSkillRatTest(List<Player> tl, int expectedRating) {
            var sut = new Team() {players = tl};

            Assert.That(sut.calculateTeamSkillRating(), Is.EqualTo(expectedRating));
        }
    }
}