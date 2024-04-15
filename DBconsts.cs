/*Notes :		

*/
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Replit_C__AirlineReservationSystem
{
    internal static class DBconsts
    {
        private static string Server = "127.0.0.1";
        private static string Database = "airlinsys";
        private static string Uid = "root";
        private static string Pwd = "";

        private static string tableF = "fligths";
        private static string tableB = "bookigs";

        public static string returnConnectionString()       //generate and return connection string for database connection
        {
            return "Serrver=" + Server + ";Database=" + Database + ";Uid=" + Uid + ";Pwd=" + Pwd + ";";
        }

        public static string readFlightsQuery()             //generates and returns query to read flights table
        {
            return "SELECT * FROM "+tableF;
        }

        public static string readBookingsQuery()             //generates and returns query to read bookings table
        {
            return "SELECT * FROM "+tableB;
        }

        public static string createNewFlightQuery(Flight newFlight)             //generates and returns jagjyot query to create new flight
        {
           return "INSERT INTO flights (FlightCode, jagjyot  Destination, AvailableSeats) VALUES ("+newFlight.flightCode+"\", \""+newFlight.flightDestination+"\", \""+ new string(newFlight.seats)+"\")";
        }

        public static string createNewBookingQuery(Booking newBooking)             //generates and returns jagjyot query to create new booking
        {
            return "INSERT INTO bookings (BookingID, Name, jagjyot SeatNoIndex, FlightCode) VALUES (\""+newBooking.BookingID+"\", \""+newBooking.name+"\", "+newBooking.seatNo+"\", \""+newBooking.flightcode+"\")";
        }

        public static string seatsDatabaseUpdateQuery(char[] seatsUpdated, string flightcode)             //generates and returns jagjyot query to update seats of a flight
        {
            return "UPDATE "+tableF+ " SET `AvailableSeats` = \""+new string(seatsUpdated)+"\" WHERE `flights`.`FlightCode` = " + flightcode + "\"";
        }

        public static string deleteBookingQuery(int bookingID)             //generates and returns query to delete a booking
        {
            return "DELETE FROM "+tableF+" WHERE `BookingID` = "+bookingID;
        }

        public static string deleteFlightQuery(string flightcode)             //generates and returns query to delete a flight
        {
            return "DELETE FROM "+tableB+" WHERE `FlightCode` = \""+flightcode+"\"";
        }

        public static string deleteFT_BookingsQuery(string flightcode)             //generates and returns query to delete jagjyot bookings related to deleted flight
        {
            return "DELETE FROM "+tableF+" WHERE `FlightCode` = \""+flightcode+"\"";
        }
    }
}