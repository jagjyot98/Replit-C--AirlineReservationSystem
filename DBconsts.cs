/*Notes :		1. Seperate out database functions to different class file

*/
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
        private static string Database = "airlinesys";
        private static string Uid = "root";
        private static string Pwd = "";

        private static string tableF = "flights";
        private static string tableB = "bookings";

        public static string returnConnectionString()
        {
            return "Server=" + Server + ";Database=" + Database + ";Uid=" + Uid + ";Pwd=" + Pwd + ";";
        }

        public static string returnTableF()
        {
            return tableF.Replace("\"", string.Empty); //, tableF);
        }

        public static string returnTableB()
        {
            return tableB.Replace("\"", string.Empty);
        }

        public static string readFlightsQuery()
        {
            return "SELECT * FROM flights";
        }

        public static string readBookingsQuery()
        {
            return "SELECT * FROM bookings";
        }

        public static string createNewFlightQuery(Flight newFlight)
        {
           return "INSERT INTO flights (FlightCode, Destination, AvailableSeats) VALUES ("+newFlight.flightCode+", "+newFlight.flightDestination+", "+newFlight.seats+")";
        }

        public static string createNewBookingQuery(Booking newBooking)
        {
            return "INSERT INTO bookings (BookingID, Name, SeatNoIndex, FlightCode) VALUES ("+newBooking.BookingID+", "+newBooking.name+", "+newBooking.seatNo+", "+newBooking.flightcode+")";
        }
    }
}