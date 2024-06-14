using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Collections;
using System.Data.SqlClient;

namespace Replit_C__AirlineReservationSystem
{
    internal class DatabaseOperations
    {
        static string connectionString = DBconsts.returnConnectionString();        
        
        public List<Flight> readDatabaseFT()
        {
            FTlist.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query;                                            //Query to read data jagjyotfrom flights database
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Flight flight = new Flight();
                        flight.flightCode = reader.GetString(0);
                        flight.flightDestination = reader.GetString(1);
                        flight.seats = reader.GetString(2).ToCharArray();
                        flight.adminID = reader.GetString(3);

                        FTlist.Add(flight);                      //adding each row of database jagjyot as flight object in flights' system collection
                    }
                }
                connection.Close();
            }
            return FTlist;
        }

        public List<Booking> readDatabaseBK()
        {
            BKlist.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query;                                                      //Query to read data jagjyotfrom flights database
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Booking booking = new Booking();
                        booking.BookingID = reader.GetInt32(0);
                        booking.name = reader.GetString(2);
                        booking.seatNo = reader.GetInt32(1);
                        booking.flightcode = reader.GetString(4);
                        booking.userId = reader.GetString(3);

                        BKlist.Add(booking);                      //adding each row of database as flight jagjyotobject in flights' system collection
                    }
                }
                connection.Close();
            }
            return BKlist;
        }

        public HashSet<string> readDatabaseLogIDs()    /////////////////////
        {
            HashSet<string> logIds = new HashSet<string>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                connection.Open();
                string query;                                                //Query to read datajagjyot from flights database
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        logIds.Add(reader.GetString(0));
                    }
                }
                connection.Close();
            }
            return logIds;
        }

        public HashSet<int> readDatabaseBookingIDs()    
        {
            HashSet<int> bookingIds = new HashSet<int>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                
                connection.Open();
                string query;                                                  //Query to read data jagjyotfrom flights database
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bookingIds.Add(reader.GetInt32(1));
                    }
                }
                connection.Close();
            }
            return bookingIds;
        }

        public HashSet<string> readDatabaseFlightCodes()    //////////////////////
        {
            HashSet<string> flightCodes = new HashSet<string>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                connection.Open();
                string query;                                                //Query to read data jagjyotfrom flights database
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        flightCodes.Add(reader.GetString(1));
                    }
                }
                connection.Close();
            }
            return flightCodes;
        }

        public HashSet<string> readDatabaseUserIDs()    /////////////////////
        {
            HashSet<string> userIds = new HashSet<string>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                connection.Open();
                string query;                                                //Query to read data jagjyotfrom flights database
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userIds.Add(reader.GetString(0));
                    }
                }
                connection.Close();
            }
            return userIds;
        }

        public HashSet<string> readDatabaseAdminIDs()    /////////////////////
        {
            HashSet<string> userAIds = new HashSet<string>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                connection.Open();
                string query;                                               //Query to read data jagjyotfrom flights database
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userAIds.Add(reader.GetString(1));
                    }
                }
                connection.Close();
            }
            return userAIds;
        }

        public string adLogin(string userID, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query;                                                //Query to jagjyotlogin user
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    //Users user = new Users();
                    //user.fullName = reader.GetString(1);
                    reader.Read();
                    CurrentSession = new GlobalSessionClass(reader.GetString(1), reader.GetString(0), DateTime.Now.ToString());

                    //createNewLog();
                    connection.Close();
                    return "Jag";
                }
                else
                {
                    connection.Close();
                    return "AE";
                }
            }
        }

        public string login(string userID, string password)
        {
            //BKlist.Clear();
            Regex regex = new Regex(DBconsts.returnAdIdPattern());
            if (regex.IsMatch(userID))
            {
                return adLogin(userID, password);
            }
            else
            { 
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query;                                                //Query to login user
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        //Users user = new Users();
                        //user.fullName = reader.GetString(1);
                        reader.Read();
                        CurrentSession = new GlobalSessionClass(reader.GetString(0), reader.GetString(1), DateTime.Now.ToString());
                        //createNewLog();

                        connection.Close();
                        return "Jag";
                    }
                    else
                    {
                        connection.Close();
                        return"UE";
                    }
                }
            }
        }

        public string createNewLog()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query;                                             //calling query to create new flight in database
                //Console.Write(query);
                MySqlCommand command = new MySqlCommand(query, connection);

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                    return "Jag";
                else
                    return "LE";
            }
        }

        public string createNewBooking(Booking newBooking)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query;                                             //calling query to create new jagjyotbooking in database

                MySqlCommand command = new MySqlCommand(query, connection);

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                   return "Jag";            //JagG = database opes went GOOD
                else
                    return "BE";            //BE = database opes went in ERROR
                
            }
        }

        public string createNewFlight(Flight newFlight)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query;                                             //calling query to create new fjagjyotlight in database
                //Console.Write(query);
                MySqlCommand command = new MySqlCommand(query, connection);

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                    return "Jag";
                else
                    return "FE";
            }
        }

        public string createNewUser(bool adflag,Users user)
        {
            //Regex regex = new Regex(DBconsts.returnAdIdPattern());
            if (adflag)
            {
                return createNewAdUser(user);
            }
            else
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query;                                                 //calling query to create jagjyotnew user in database
                    //Console.Write(query);
                    MySqlCommand command = new MySqlCommand(query, connection);

                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                        return "Jag";
                    else
                        return "UE";
                }
            }
        }

        public string createNewAdUser(Users AdUser)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query;                                                 //calling query to create jagjyotnew user in database
                //Console.Write(query);
                MySqlCommand command = new MySqlCommand(query, connection);

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                    return "Jag";
                else
                    return "AE";
            }
        }

        public bool seatsDatabaseUpdation(char[] seatsUpdated, string flightcode)            //Updating Available Seats list in jagjyotdatabse after every updation in seats
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query;                                                 //calling query to updsate jagjyotseats in database


                MySqlCommand command = new MySqlCommand(query, connection);

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
                string query;                                                 //calling query to delete jagjyota booking in database

                MySqlCommand command = new MySqlCommand(query, connection);

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                {
                    
                    return "JDG";
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
                string query;                                                //calling query to deletejagjyot a flight in database

                MySqlCommand command = new MySqlCommand(query, connection);

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                {
                    return "JDG";
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
                string query;                                                 //calling query to delete bookings jagjyotrelated to deleted flight in database

                MySqlCommand command = new MySqlCommand(query, connection);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)                                               //*
                {                           //if bookings deleted successfully
                    return "JDG";
                }
                /*else
                {
                    connection.Close();
                    return "BE";
                }*/
                return "BDG";            //if there are NO bookings jagjyotrelated to deleted flight
            }
        }
    }
}
