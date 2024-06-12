﻿/*Notes :		

*/
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Replit_C__AirlineReservationSystem
{
    internal static class DBconsts
    {
        private static string Server = "127.0.0.1";
        private static string Database = "airlinesys";
        private static string Uid = "root";
        private static string Pwd = "";

        private static string tableU = "users";
        private static string tableA = "admin";
        private static string tableF = "flights";
        private static string tableB = "bookings";
        private static string tableL = "LoginLogs";

        private static string uIdpattern = @"^UN\d{5}$";
        private static string adIdpattern = @"^[A-Z]{2}\d{3}[A-Z]{3}\d{3}[A-Z]{2}$";
        private static string uPassPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$";
        
        private static string destinationPattern = @"^[a-zA-Z]{3,30}$";
        private static string flightCodePattern = @"^FL\d{4}$";
        private static string bookingIdPattern = @"^\d{3}$";

        private static HashSet<string> usedAdminIDs = new HashSet<string>();

        public static string generateAdminID(string name)
        {
            DatabaseOperations DBops = new DatabaseOperations();
            usedAdminIDs = DBops.readDatabaseAdminIDs();              ////////////////////

            string randomAId;
            Random rand = new Random();
            do
            {
                randomAId = "UN" + rand.Next(100, 999) + name.ToUpper().Substring(0, 3) + rand.Next(100, 999) + "AD";
            } while (usedAdminIDs.Contains(randomAId));

            return randomAId;
        }

        public static string returnConnectionString()       //generate and return connection string for database connection
        {
            return "Server=" + Server + ";Database=" + Database + ";Uid=" + Uid + ";Pwd=" + Pwd + ";";
        }

        public static string returnAdIdPattern()
        {
            return adIdpattern;
        }

        public static string returnUIdPattern()
        {
            return uIdpattern;
        }

        public static string returnUPassPattern()
        {
            return uPassPattern;
        }

        public static string returnDestPattern()
        {
            return destinationPattern;
        }

        public static string returnflightCodePattern()
        {
            return flightCodePattern;
        }

        public static string returnbookingIdPattern()
        {
            return bookingIdPattern;
        }

        public static string readLoginQuery(string userID, string password) 
        {
            return "SELECT * FROM "+tableU+" WHERE `userID` = \""+userID+"\" AND `password` = \""+password+"\"";
        }

        public static string readAdLoginQuery(string userID, string password)
        {
            return "SELECT * FROM " + tableA + " WHERE `adminID` = \"" + userID + "\" AND `adminPassword` = \"" + password + "\"";
        }

        public static string readFlightsQuery()             //generates and returns query to read flights table
        {
            return "SELECT * FROM "+tableF;
        }

        public static string readBookingsQuery()             //generates and returns query to read bookings table
        {
            return "SELECT * FROM " + tableB;
        }

        public static string readLogIDsQuery()             //generates and returns query to read bookingIDs
        {
            return "SELECT `LogID` FROM " + tableL;
        }

        public static string readBookingIDsQuery()             //generates and returns query to read bookingIDs
        {
            return "SELECT `BookingID` FROM " + tableB;
        }

        public static string readFlightCodesQuery()             //generates and returns query to read flightCodes
        {
           return "SELECT `FlightCode` FROM " + tableF;
        }

        public static string readAdminIDsQuery()             //generates and returns query to read adminIDs
        {
           return "SELECT `adminID` FROM " + tableA;
        }

        public static string readUserIDsQuery()             //generates and returns query to read adminIDs
        {
           return "SELECT `userID` FROM " + tableU;
        }

        public static string createNewLogQuery()             //generates and returns query to create new User
        {
            return "INSERT INTO " + tableL + " (LogID, LogInTimeStamp, LogOutTimeStamp, LogUserID, LogUserName) VALUES (\"" + GlobalSessionClass.LogID + "\", \"" + GlobalSessionClass.LogInTimeStamp + "\", \"" + GlobalSessionClass.LogOutTimeStamp + "\", \"" + GlobalSessionClass.currentUserID + "\", \"" + GlobalSessionClass.currentUserName + "\")";
        }

        public static string createNewUserQuery(Users newUser)             //generates and returns query to create new User
        {
            return "INSERT INTO "+tableU+" (userID, FullName, Password, signUpTimeStamp) VALUES (\"" + newUser.userID + "\", \"" + newUser.fullName + "\", \"" + newUser.password + "\",\""+newUser.signUpTimeStamp+ "\")";
        }

        public static string createNewAdUserQuery(Users newAdUser, string CreatorAdminID)             //generates and returns query to create new adUser
        {
            return "INSERT INTO " + tableA + " (adminID, CreatorAdminID, adminName, adminPassword, signUpTimeStamp, SignUpLogId) VALUES (\"" + newAdUser.userID + "\", \"" + CreatorAdminID + "\", \"" + newAdUser.fullName + "\", \"" + newAdUser.password + "\",\"" + newAdUser.signUpTimeStamp + "\",\"" + GlobalSessionClass.LogID + "\")";
        }

        public static string createNewFlightQuery(Flight newFlight)             //generates and returns query to create new flight
        {
           return "INSERT INTO "+tableF+" (FlightCode, AdminId, Destination, AvailableSeats, FcreationTimeStamp, FCreationLogID) VALUES (\""+newFlight.flightCode+ "\", \""+newFlight.adminID+"\", \"" + newFlight.flightDestination+"\", \""+ new string(newFlight.seats)+"\",\"" + newFlight.FcreationTimeStamp + "\",\"" + GlobalSessionClass.LogID + "\")";
        }

        public static string createNewBookingQuery(Booking newBooking)             //generates and returns query to create new booking
        {
            return "INSERT INTO "+tableB+" (BookingID, UserID, Name, SeatNoIndex, FlightCode, BcreationTimeStamp, BCreationLogID) VALUES (\""+newBooking.BookingID+ "\", \""+newBooking.userId+"\", \"" + newBooking.name+"\", \""+newBooking.seatNo+"\", \""+newBooking.flightcode+ "\", \"" + newBooking.BcreationTimeStamp + "\",\"" + GlobalSessionClass.LogID + "\")";
        }

        public static string seatsDatabaseUpdateQuery(char[] seatsUpdated, string flightcode)             //generates and returns query to update seats of a flight
        {
            return "UPDATE "+tableF+ " SET `AvailableSeats` = \""+new string(seatsUpdated)+"\" WHERE `flights`.`FlightCode` = \"" + flightcode + "\"";
        }

        public static string deleteBookingQuery(int bookingID, string userID)             //generates and returns query to delete a booking
        {
            return "DELETE FROM "+tableB+" WHERE `BookingID` = \""+bookingID+"\"AND `UserID` = \""+userID+"\"";
        }

        public static string deleteFlightQuery(string flightcode, string adminID)             //generates and returns query to delete a flight
        {
            return "DELETE FROM "+tableF+" WHERE `FlightCode` = \""+flightcode+ "\"AND `AdminID` = \""+adminID+"\"";
        }

        public static string deleteFT_BookingsQuery(string flightcode)             //generates and returns query to delete bookings relatted to deleted flight
        {
            return "DELETE FROM "+tableB+" WHERE `FlightCode` = \""+flightcode+"\"";
        }
    }
}