using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Replit_C__AirlineReservationSystem
{
    internal class GlobalSessionClass
    {
        public static string LogID;
        public static string LogInTimeStamp;
        public static string LogOutTimeStamp;
        public static string currentUserID;
        public static string currentUserName;

        private static HashSet<string> usedLogIds = new HashSet<string>();

        public GlobalSessionClass() { }
        public GlobalSessionClass(string UserID, string Username, string LogInTime)
        {
            DatabaseOperations DBops = new DatabaseOperations();
            usedLogIds = DBops.readDatabaseLogIDs();

            string randomCode;
            Random rand = new Random();
            do
            {
                randomCode = "LG" + rand.Next(1000, 9999);
            } while (usedLogIds.Contains(randomCode));

            LogID = randomCode;

            currentUserID = UserID;
            currentUserName = Username;
            LogInTimeStamp = LogInTime;
        }
    }
}
