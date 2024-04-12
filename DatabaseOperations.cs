/*Notes :		1. Seperate out database functions to different class file

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Collections;

namespace Replit_C__AirlineReservationSystem
{
    internal class DatabaseOperations
    {
        static string connectionString = DBconsts.returnConnectionString();        //"Server=127.0.0.1;Database=airlinesys;Uid=root;Pwd=;"
        
        List<Flight> FTlist = new List<Flight>();
        List<Booking> BKlist = new List<Booking>();   

        public List<Flight> readDatabaseFT()
        {
            FTlist.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = DBconsts.readFlightsQuery();                         //Query to read data from flights database
                MySqlCommand command = new MySqlCommand(query, connection);
                //command.Parameters.AddWithValue("@DatabaseTable", DBconsts.returnTableF());
                Console.Write(command);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Flight flight = new Flight();
                        flight.flightCode = reader.GetString(0);
                        flight.flightDestination = reader.GetString(1);
                        flight.seats = reader.GetString(2).ToCharArray();

                        FTlist.Add(flight);                      //adding each row of database as flight object in flights' system collection
                    }
                }
                connection.Close();
            }
            return FTlist;
        }

        public List<Booking> readDatabaseBK()
        {
            FTlist.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = DBconsts.readBookingsQuery();                         //Query to read data from flights database
                MySqlCommand command = new MySqlCommand(query, connection);
                //command.Parameters.AddWithValue("@DatabaseTable", DBconsts.returnTableB());

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Booking booking = new Booking();
                        booking.BookingID = reader.GetInt32(0);
                        booking.name = reader.GetString(1);
                        booking.seatNo = reader.GetInt32(2);
                        booking.flightcode = reader.GetString(3);

                        BKlist.Add(booking);                      //adding each row of database as flight object in flights' system collection
                    }
                }
                connection.Close();
            }
            return BKlist;
        }

        public string createNewBooking(Booking newBooking)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = DBconsts.createNewBookingQuery(newBooking);

                MySqlCommand command = new MySqlCommand(query, connection);
                /*command.Parameters.AddWithValue("@BookingID", newBooking.BookingID);
                command.Parameters.AddWithValue("@Name", newBooking.name);
                command.Parameters.AddWithValue("@SeatNoIndex", newBooking.seatNo);
                command.Parameters.AddWithValue("@FlightCode", newBooking.flightcode);*/

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                   return "BG";            //BG = database opes went GOOD
                else
                    return "BE";            //BE = database opes went in ERROR
                
            }
        }

        public string createNewFlight(Flight newFlight)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = DBconsts.createNewFlightQuery(newFlight);

                MySqlCommand command = new MySqlCommand(query, connection);
                /*command.Parameters.AddWithValue("@FlightCode", newFlight.flightCode);
                command.Parameters.AddWithValue("@Destination", newFlight.flightDestination);
                command.Parameters.AddWithValue("@AvailableSeats", newFlight.seats);*/              /////////////////ERROR 

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                    return "FG";
                else
                    return "FE";
            }
        }

        public bool seatsDatabaseUpdation(char[] seatsUpdated, string flightcode)            //Updating Available Seats list in databse after every updation in seats
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE @DatabaseTable SET `AvailableSeats` = @AvailableSeats WHERE `flights`.`FlightCode` = @FlightCode";


                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@DatabaseTable", DBconsts.returnTableF());
                command.Parameters.AddWithValue("@FlightCode", flightcode);
                command.Parameters.AddWithValue("@AvailableSeats", new string(seatsUpdated));

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();
                if (rowsAffected > 0)
                    return true;
                else
                    return false;
            }
        }

        public string deleteBooking(int BookingID)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))      //deleting booking from database
            {
                connection.Open();
                string query = "DELETE FROM @DatabaseTable WHERE `BookingID` = @BookingID";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@DatabaseTable", DBconsts.returnTableB());
                command.Parameters.AddWithValue("@BookingID", BookingID);

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                {
                    
                    return "BDG";
                }
                else
                {
                    return "BDE";
                }

            }
        }

        public string deleteFlight(string FlightCode)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))      //deleting booking from database
            {
                connection.Open();
                string query = "DELETE FROM @DatabaseTable WHERE `FlightCode` = @FlightCode";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@DatabaseTable", DBconsts.returnTableF());
                command.Parameters.AddWithValue("@FlightCode", FlightCode);

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                {
                    return "FDG";
                }
                else
                {                                                       //if error occurs in flight deletion
                    return "FDE";
                }

            }
        }

        public string deleteFT_Bookings(string flightCode)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM @DatabaseTable WHERE `FlightCode` = @FlightCode";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@DatabaseTable", DBconsts.returnTableB());
                command.Parameters.AddWithValue("@FlightCode", flightCode);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)                                               //*
                {                           //if bookings deleted successfully
                    return "BDG";
                }
                /*else
                {
                    connection.Close();
                    return "BE";
                }*/
                return "BDG";            //if there are NO bookings related to deleted flight
            }
        }
    }
}
