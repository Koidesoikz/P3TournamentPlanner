using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using P3TournamentPlanner.Shared;

namespace P3TournamentPlanner.Client.Services
{
    public class TeamService
    {
        public event Action RefreshRequested;
        public event Action RerenderRequested;

        public void CallRequestRefresh()
        {
            RefreshRequested?.Invoke();
        }
        public void CallRequestRerender()
        {
            RerenderRequested?.Invoke();
        }
    }
}
