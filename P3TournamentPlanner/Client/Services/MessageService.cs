using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P3TournamentPlanner.Client.Services
{
    public class MessageService
    {
        public event Action RefreshRequested;
        public event Action RerenderRequested;

        private static System.Timers.Timer aTimer;
        public string messageDisplay { get; set; } = "none";
        public string messageStyle { get; set; }
        public string message { get; set; }
        public void showMessage(string style, string m)
        {
            messageDisplay = "block";
            messageStyle = style;
            message = m;
            Console.WriteLine(messageDisplay);

            aTimer = new System.Timers.Timer(5000);
            aTimer.Enabled = true;
            aTimer.Elapsed += CountDownTimer;
            CallRequestRefresh();
        }

        public void CountDownTimer(Object source, System.Timers.ElapsedEventArgs e)
        {
            messageDisplay = "none";
            aTimer.Enabled = false;
            CallRequestRefresh();
        }

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
